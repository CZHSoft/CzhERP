<script lang="ts" setup name="stoStockOut">
import { ref, reactive, onMounted } from "vue";
import { auth } from '/@/utils/authFunction';
import { ElMessageBox, ElMessage } from "element-plus";
import { formatDate } from "/@/utils/formatTime";
import { downloadStreamFile } from "/@/utils/download";
import { useStoStockOutApi } from '/@/api/stock/stoStockOut';
import editDialog from '/@/views/stock/stoStockOut/component/editDialog.vue'
import printDialog from '/@/views/system/print/component/hiprint/preview.vue'
import ModifyRecord from '/@/components/table/modifyRecord.vue';
import ImportData from "/@/components/table/importData.vue";

const stoStockOutApi = useStoStockOutApi();
const printDialogRef = ref();
const editDialogRef = ref();
const importDataRef = ref();
const state = reactive({
  exportLoading: false,
  tableLoading: false,
  stores: {},
  showAdvanceQueryUI: false,
  dropdownData: {} as any,
  selectData: [] as any[],
  tableQueryParams: {} as any,
  tableParams: {
    page: 1,
    pageSize: 20,
    total: 0,
    field: 'createTime',
    order: 'descending',
    descStr: 'descending',
  },
  tableData: [],
  statusType: {
    Draft: { text: '草稿', type: 'info' },
    Approved: { text: '已审批', type: 'warning' },
    Confirmed: { text: '已确认', type: 'success' },
    Cancelled: { text: '已取消', type: 'danger' },
  },
  stockOutType: {
    Sale: { text: '销售出库', type: 'primary' },
    Issue: { text: '领料出库', type: 'success' },
    Transfer: { text: '调拨出库', type: 'warning' },
    Other: { text: '其他出库', type: 'info' },
  },
});

onMounted(async () => {
});

// 获取状态显示文本
const getStatusText = (status: string) => {
  return state.statusType[status]?.text || status;
};

// 获取出库类型显示文本
const getStockOutTypeText = (type: string) => {
  return state.stockOutType[type]?.text || type;
};

// 查询操作
const handleQuery = async (params: any = {}) => {
  state.tableLoading = true;
  state.tableParams = Object.assign(state.tableParams, params);
  const result = await stoStockOutApi.page(Object.assign(state.tableQueryParams, state.tableParams)).then(res => res.data.result);
  state.tableParams.total = result?.total;
  state.tableData = result?.items || [];
  state.tableLoading = false;
};

// 列排序
const sortChange = async (column: any) => {
  state.tableParams.field = column.prop;
  state.tableParams.order = column.order;
  await handleQuery();
};

// 删除
const delStoStockOut = (row: any) => {
  ElMessageBox.confirm(`确定要删除出库单【${row.stockOutNo}】吗?`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  }).then(async () => {
    await stoStockOutApi.delete({ id: row.id });
    handleQuery();
    ElMessage.success("删除成功");
  }).catch(() => {});
};

// 批量删除
const batchDelStoStockOut = () => {
  ElMessageBox.confirm(`确定要删除${state.selectData.length}条记录吗?`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  }).then(async () => {
    await stoStockOutApi.batchDelete(state.selectData.map(u => ({ id: u.id })));
    ElMessage.success(`成功批量删除${state.selectData.length}条记录`);
    handleQuery();
  }).catch(() => {});
};

// 审核出库
const approveStoStockOut = (row: any) => {
  ElMessageBox.confirm(`确定要审核出库单【${row.stockOutNo}】吗？\n审核通过后将扣减库存并更新关联单据状态！`, "审核出库", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  }).then(async () => {
    const res = await stoStockOutApi.approve({ id: row.id });
    ElMessage.success(res.data.result || '审核成功！');
    handleQuery();
  }).catch(() => {});
};

// 拒绝出库
const rejectStoStockOut = (row: any) => {
  ElMessageBox.prompt('请输入拒绝原因', '拒绝出库', {
    confirmButtonText: '确定',
    cancelButtonText: '取消',
    inputErrorMessage: '拒绝原因不能为空',
    inputValidator: (value: string) => {
      if (!value || !value.trim()) {
        return '请输入拒绝原因';
      }
      return true;
    }
  }).then(async ({ value }) => {
    const res = await stoStockOutApi.reject({ 
      id: row.id,
      rejectReason: value
    });
    ElMessage.success(res.data.result || '已拒绝！');
    handleQuery();
  }).catch(() => {});
};

// 导出数据
const exportStoStockOutCommand = async (command: string) => {
  try {
    state.exportLoading = true;
    if (command === 'select') {
      const params = Object.assign({}, state.tableQueryParams, state.tableParams, { selectKeyList: state.selectData.map(u => u.id) });
      await stoStockOutApi.exportData(params).then(res => downloadStreamFile(res));
    } else if (command === 'current') {
      const params = Object.assign({}, state.tableQueryParams, state.tableParams);
      await stoStockOutApi.exportData(params).then(res => downloadStreamFile(res));
    } else if (command === 'all') {
      const params = Object.assign({}, state.tableQueryParams, state.tableParams, { page: 1, pageSize: 99999999 });
      await stoStockOutApi.exportData(params).then(res => downloadStreamFile(res));
    }
  } finally {
    state.exportLoading = false;
  }
}

handleQuery();
</script>
<template>
  <div class="stoStockOut-container" v-loading="state.exportLoading">
    <el-card shadow="hover" :body-style="{ paddingBottom: '0' }"> 
      <el-form :model="state.tableQueryParams" ref="queryForm" labelWidth="90">
        <el-row>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10">
            <el-form-item label="关键字">
              <el-input v-model="state.tableQueryParams.keyword" clearable placeholder="请输入模糊查询关键字"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="出库单号">
              <el-input v-model="state.tableQueryParams.stockOutNo" clearable placeholder="请输入出库单号"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="出库类型">
              <el-select v-model="state.tableQueryParams.stockOutType" placeholder="请选择出库类型" clearable style="width: 100%;">
                <el-option label="销售出库" value="Sale" />
                <el-option label="领料出库" value="Issue" />
                <el-option label="调拨出库" value="Transfer" />
                <el-option label="其他出库" value="Other" />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="仓库名称">
              <el-input v-model="state.tableQueryParams.warehouseName" clearable placeholder="请输入仓库名称"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="来源单据号">
              <el-input v-model="state.tableQueryParams.sourceBillNo" clearable placeholder="请输入来源单据号"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="出库日期">
              <el-date-picker type="daterange" v-model="state.tableQueryParams.stockOutDateRange" value-format="YYYY-MM-DD HH:mm:ss" start-placeholder="开始日期" end-placeholder="结束日期" :default-time="[new Date('1 00:00:00'), new Date('1 23:59:59')]" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="状态">
              <el-select v-model="state.tableQueryParams.status" placeholder="请选择状态" clearable style="width: 100%;">
                <el-option label="草稿" value="Draft" />
                <el-option label="已审批" value="Approved" />
                <el-option label="已确认" value="Confirmed" />
                <el-option label="已取消" value="Cancelled" />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="备注">
              <el-input v-model="state.tableQueryParams.remark" clearable placeholder="请输入备注"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10">
            <el-form-item >
              <el-button-group style="display: flex; align-items: center;">
                <el-button type="primary" icon="ele-Search" @click="handleQuery" v-auth="'stoStockOut:page'" v-reclick="1000"> 查询 </el-button>
                <el-button icon="ele-Refresh" @click="() => state.tableQueryParams = {}"> 重置 </el-button>
                <el-button icon="ele-ZoomIn" @click="() => state.showAdvanceQueryUI = true" v-if="!state.showAdvanceQueryUI" style="margin-left:5px;"> 高级查询 </el-button>
                <el-button icon="ele-ZoomOut" @click="() => state.showAdvanceQueryUI = false" v-if="state.showAdvanceQueryUI" style="margin-left:5px;"> 隐藏 </el-button>
              </el-button-group>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb10">
            <el-form-item >
              <el-button-group style="display: flex; align-items: center;">
                <el-button type="primary" icon="ele-Plus" @click="editDialogRef.openDialog(null, '新增出库单')" v-auth="'stoStockOut:add'" v-reclick="1000"> 新增 </el-button>
                <el-button type="danger" icon="ele-Delete" @click="batchDelStoStockOut" :disabled="state.selectData.length == 0" v-auth="'stoStockOut:batchDelete'" style="margin-left:5px;"> 删除 </el-button>
                <el-dropdown :show-timeout="70" :hide-timeout="50" @command="exportStoStockOutCommand" style="margin-left:5px;">
                  <el-button type="primary" icon="ele-FolderOpened" v-reclick="20000" v-auth="'stoStockOut:export'"> 导出 </el-button>
                  <template #dropdown>
                    <el-dropdown-menu>
                      <el-dropdown-item command="select" :disabled="state.selectData.length == 0">导出选中</el-dropdown-item>
                      <el-dropdown-item command="current">导出本页</el-dropdown-item>
                      <el-dropdown-item command="all">导出全部</el-dropdown-item>
                    </el-dropdown-menu>
                  </template>
                </el-dropdown>
                <el-button type="warning" icon="ele-Upload" @click="importDataRef.openDialog()" v-auth="'stoStockOut:import'" style="margin-left:5px;"> 导入 </el-button>
              </el-button-group>
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
    </el-card>
    <el-card class="full-table" shadow="hover" style="margin-top: 5px">
      <el-table :data="state.tableData" @selection-change="(val: any[]) => { state.selectData = val; }" style="width: 100%" v-loading="state.tableLoading" tooltip-effect="light" row-key="id" @sort-change="sortChange" border>
        <el-table-column type="selection" width="40" align="center" v-if="auth('stoStockOut:batchDelete') || auth('stoStockOut:export')" />
        <el-table-column type="index" label="序号" width="55" align="center"/>
        <el-table-column prop="stockOutNo" label="出库单号" width="150" show-overflow-tooltip />
        <el-table-column prop="stockOutType" label="出库类型" width="120" show-overflow-tooltip>
          <template #default="scope">
            {{ getStockOutTypeText(scope.row.stockOutType) }}
          </template>
        </el-table-column>
        <el-table-column prop="warehouseName" label="仓库" width="120" show-overflow-tooltip />
        <el-table-column prop="sourceBillNo" label="来源单据" width="150" show-overflow-tooltip />
        <el-table-column prop="stockOutDate" label="出库日期" width="120" show-overflow-tooltip>
          <template #default="scope">
            {{ scope.row.stockOutDate ? formatDate(scope.row.stockOutDate) : '-' }}
          </template>
        </el-table-column>
        <el-table-column prop="totalQuantity" label="数量" width="80" align="right" show-overflow-tooltip />
        <el-table-column prop="totalAmount" label="金额" width="100" align="right" show-overflow-tooltip>
          <template #default="scope">
            {{ scope.row.totalAmount?.toFixed(2) }}
          </template>
        </el-table-column>
        <el-table-column prop="status" label="状态" width="100" align="center" show-overflow-tooltip>
          <template #default="scope">
            <el-tag :type="state.statusType[scope.row.status]?.type || 'info'" size="small">
              {{ getStatusText(scope.row.status) }}
            </el-tag>
          </template>
        </el-table-column>
        <el-table-column prop="remark" label="备注" show-overflow-tooltip />
        <el-table-column label="修改记录" width="80" align="center" show-overflow-tooltip>
          <template #default="scope">
            <ModifyRecord :data="scope.row" />
          </template>
        </el-table-column>
        <el-table-column label="操作" width="180" align="center" fixed="right" show-overflow-tooltip v-if="auth('stoStockOut:update') || auth('stoStockOut:delete') || auth('stoStockOut:approve')">
          <template #default="scope">
            <el-button v-if="scope.row.status === 'Draft'" type="success" size="small" text icon="ele-Check" @click="approveStoStockOut(scope.row)" v-auth="'stoStockOut:approve'"> 审核 </el-button>
            <el-button v-if="scope.row.status === 'Draft'" type="danger" size="small" text icon="ele-X" @click="rejectStoStockOut(scope.row)" v-auth="'stoStockOut:reject'"> 拒绝 </el-button>
            <el-button icon="ele-Edit" size="small" text type="primary" @click="editDialogRef.openDialog(scope.row, '编辑出库单')" v-auth="'stoStockOut:update'" :disabled="scope.row.status !== 'Draft'"> 编辑 </el-button>
            <el-button icon="ele-Delete" size="small" text type="danger" @click="delStoStockOut(scope.row)" v-auth="'stoStockOut:delete'" :disabled="scope.row.status !== 'Draft'"> 删除 </el-button>
          </template>
        </el-table-column>
      </el-table>
      <el-pagination 
              v-model:currentPage="state.tableParams.page"
              v-model:page-size="state.tableParams.pageSize"
              @size-change="(val: any) => handleQuery({ pageSize: val })"
              @current-change="(val: any) => handleQuery({ page: val })"
              layout="total, sizes, prev, pager, next, jumper"
              :page-sizes="[10, 20, 50, 100, 200, 500]"
              :total="state.tableParams.total"
              size="small"
              background />
      <ImportData ref="importDataRef" :import="stoStockOutApi.importData" :download="stoStockOutApi.downloadTemplate" v-auth="'stoStockOut:import'" @refresh="handleQuery"/>
      <printDialog ref="printDialogRef" :title="'打印出库单'" @reloadTable="handleQuery" />
      <editDialog ref="editDialogRef" @reloadTable="handleQuery" />
    </el-card>
  </div>
</template>
<style scoped>
:deep(.el-input), :deep(.el-select), :deep(.el-input-number) {
  width: 100%;
}
</style>