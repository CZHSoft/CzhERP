<script lang="ts" setup name="stoStockIn">
import { ref, reactive, onMounted } from "vue";
import { auth } from '/@/utils/authFunction';
import { ElMessageBox, ElMessage } from "element-plus";
import { downloadStreamFile } from "/@/utils/download";
import { useStoStockInApi } from '/@/api/stock/stoStockIn';
import editDialog from '/@/views/stock/stoStockIn/component/editDialog.vue'
import mockStockInDialog from '/@/views/stock/stoStockIn/component/mockStockInDialog.vue'
import printDialog from '/@/views/system/print/component/hiprint/preview.vue'
import ModifyRecord from '/@/components/table/modifyRecord.vue';
import ImportData from "/@/components/table/importData.vue";

const stoStockInApi = useStoStockInApi();
const printDialogRef = ref();
const editDialogRef = ref();
const mockStockInDialogRef = ref();
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
    field: 'createTime', // 默认的排序字段
    order: 'descending', // 排序方向
    descStr: 'descending', // 降序排序的关键字符
  },
  tableData: [],
});

// 页面加载时
onMounted(async () => {
});

// 查询操作
const handleQuery = async (params: any = {}) => {
  state.tableLoading = true;
  state.tableParams = Object.assign(state.tableParams, params);
  const result = await stoStockInApi.page(Object.assign(state.tableQueryParams, state.tableParams)).then(res => res.data.result);
  state.tableParams.total = result?.total;
  state.tableData = result?.items ?? [];
  state.tableLoading = false;
};

// 列排序
const sortChange = async (column: any) => {
  state.tableParams.field = column.prop;
  state.tableParams.order = column.order;
  await handleQuery();
};

// 批量确认入库
const batchConfirmStockIn = () => {
  const selected = state.selectData.filter(item => item.status === 'Draft' || item.status === 'Approved');
  if (selected.length === 0) {
    ElMessage.warning('请选择草稿或已审批状态的入库单进行确认！');
    return;
  }
  ElMessageBox.confirm(`确定要确认选中的 ${selected.length} 条入库单吗？确认后将自动更新库存数据。`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  }).then(async () => {
    let successCount = 0;
    let failCount = 0;
    for (const row of selected) {
      try {
        await stoStockInApi.confirm({ id: row.id });
        successCount++;
      } catch (error: any) {
        failCount++;
        console.error(`入库单${row.stockInNo}确认失败：`, error.message);
      }
    }
    if (failCount > 0) {
      ElMessage.warning(`批量确认完成：成功 ${successCount} 条，失败 ${failCount} 条`);
    } else {
      ElMessage.success(`批量确认成功！共 ${successCount} 条`);
    }
    handleQuery();
  }).catch(() => {});
};

// 入库确认
const confirmStockIn = (row: any) => {
  ElMessageBox.confirm(`确定要确认入库单【${row.stockInNo}】吗？确认后将自动更新库存数据。`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  }).then(async () => {
    try {
      const res = await stoStockInApi.confirm({ id: row.id });
      ElMessage.success(res.data.result || "确认成功！");
      handleQuery();
    } catch (error: any) {
      ElMessage.error(error.message || "确认失败！");
    }
  }).catch(() => {});
};

// 取消入库确认
const cancelConfirmStockIn = (row: any) => {
  ElMessageBox.confirm(`确定要取消入库单【${row.stockInNo}】的确认吗？取消后将回滚库存数据。`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  }).then(async () => {
    try {
      const res = await ElMessageBox.prompt('请输入取消原因', '取消确认', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        inputErrorMessage: '取消原因不能为空',
      });
      const res2 = await stoStockInApi.cancelConfirm({ id: row.id, cancelReason: res.value });
      ElMessage.success(res2.data.result || "取消确认成功！");
      handleQuery();
    } catch (error: any) {
      if (error !== 'cancel') {
        ElMessage.error(error.message || "取消确认失败！");
      }
    }
  }).catch(() => {});
};

handleQuery();

// 删除
const delStoStockIn = (row: any) => {
  ElMessageBox.confirm(`确定要删除吗?`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  }).then(async () => {
    await stoStockInApi.delete({ id: row.id });
    handleQuery();
    ElMessage.success("删除成功");
  }).catch(() => {});
};

// 批量删除
const batchDelStoStockIn = () => {
  ElMessageBox.confirm(`确定要删除${state.selectData.length}条记录吗?`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  }).then(async () => {
    await stoStockInApi.batchDelete(state.selectData.map(u => ({ id: u.id }) )).then(res => {
      ElMessage.success(`成功批量删除${res.data.result}条记录`);
      handleQuery();
    });
  }).catch(() => {});
};

// 导出数据
const exportStoStockInCommand = async (command: string) => {
  try {
    state.exportLoading = true;
    if (command === 'select') {
      const params = Object.assign({}, state.tableQueryParams, state.tableParams, { selectKeyList: state.selectData.map(u => u.id) });
      await stoStockInApi.exportData(params).then(res => downloadStreamFile(res));
    } else if (command === 'current') {
      const params = Object.assign({}, state.tableQueryParams, state.tableParams);
      await stoStockInApi.exportData(params).then(res => downloadStreamFile(res));
    } else if (command === 'all') {
      const params = Object.assign({}, state.tableQueryParams, state.tableParams, { page: 1, pageSize: 99999999 });
      await stoStockInApi.exportData(params).then(res => downloadStreamFile(res));
    }
  } finally {
    state.exportLoading = false;
  }
}

handleQuery();
</script>
<template>
  <div class="stoStockIn-container" v-loading="state.exportLoading">
    <el-card shadow="hover" :body-style="{ paddingBottom: '0' }"> 
      <el-form :model="state.tableQueryParams" ref="queryForm" labelWidth="90">
        <el-row>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10">
            <el-form-item label="关键字">
              <el-input v-model="state.tableQueryParams.keyword" clearable placeholder="请输入模糊查询关键字"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="入库单号">
              <el-input v-model="state.tableQueryParams.stockInNo" clearable placeholder="请输入入库单号"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="入库类型">
              <el-input v-model="state.tableQueryParams.stockInType" clearable placeholder="请输入入库类型"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="入库仓库ID">
              <el-input v-model="state.tableQueryParams.warehouseId" clearable placeholder="请输入入库仓库ID"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="仓库编码">
              <el-input v-model="state.tableQueryParams.warehouseCode" clearable placeholder="请输入仓库编码"/>
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
            <el-form-item label="入库日期">
              <el-date-picker type="daterange" v-model="state.tableQueryParams.stockInDateRange"  value-format="YYYY-MM-DD HH:mm:ss" start-placeholder="开始日期" end-placeholder="结束日期" :default-time="[new Date('1 00:00:00'), new Date('1 23:59:59')]" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="入库总数量">
              <el-input-number v-model="state.tableQueryParams.totalQuantity"  clearable placeholder="请输入入库总数量"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="入库总金额">
              <el-input-number v-model="state.tableQueryParams.totalAmount"  clearable placeholder="请输入入库总金额"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="状态">
              <el-input v-model="state.tableQueryParams.status" clearable placeholder="请输入状态"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="审批人ID">
              <el-input v-model="state.tableQueryParams.approvalUserId" clearable placeholder="请输入审批人ID"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="审批时间">
              <el-date-picker type="daterange" v-model="state.tableQueryParams.approvalTimeRange"  value-format="YYYY-MM-DD HH:mm:ss" start-placeholder="开始日期" end-placeholder="结束日期" :default-time="[new Date('1 00:00:00'), new Date('1 23:59:59')]" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="审批意见">
              <el-input v-model="state.tableQueryParams.approvalRemark" clearable placeholder="请输入审批意见"/>
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
                <el-button type="primary"  icon="ele-Search" @click="handleQuery" v-auth="'stoStockIn:page'" v-reclick="1000"> 查询 </el-button>
                <el-button icon="ele-Refresh" @click="() => state.tableQueryParams = {}"> 重置 </el-button>
                <el-button icon="ele-ZoomIn" @click="() => state.showAdvanceQueryUI = true" v-if="!state.showAdvanceQueryUI" style="margin-left:5px;"> 高级查询 </el-button>
                <el-button icon="ele-ZoomOut" @click="() => state.showAdvanceQueryUI = false" v-if="state.showAdvanceQueryUI" style="margin-left:5px;"> 隐藏 </el-button>
                <el-button type="danger" style="margin-left:5px;" icon="ele-Delete" @click="batchDelStoStockIn" :disabled="state.selectData.length == 0" v-auth="'stoStockIn:batchDelete'"> 删除 </el-button>
                <el-button type="primary" style="margin-left:5px;" icon="ele-Plus" @click="editDialogRef.openDialog(null, '新增入库单主表')" v-auth="'stoStockIn:add'"> 新增 </el-button>
                <el-button type="success" style="margin-left:5px;" icon="ele-MagicStick" @click="mockStockInDialogRef.openDialog()" v-auth="'stoStockIn:add'"> 批次入库 </el-button>
                <el-button type="warning" style="margin-left:5px;" icon="ele-Check" @click="batchConfirmStockIn" :disabled="state.selectData.length == 0" v-auth="'stoStockIn:update'"> 批量确认 </el-button>
                <el-dropdown :show-timeout="70" :hide-timeout="50" @command="exportStoStockInCommand">
                  <el-button type="primary" style="margin-left:5px;" icon="ele-FolderOpened" v-reclick="20000" v-auth="'stoStockIn:export'"> 导出 </el-button>
                  <template #dropdown>
                    <el-dropdown-menu>
                      <el-dropdown-item command="select" :disabled="state.selectData.length == 0">导出选中</el-dropdown-item>
                      <el-dropdown-item command="current">导出本页</el-dropdown-item>
                      <el-dropdown-item command="all">导出全部</el-dropdown-item>
                    </el-dropdown-menu>
                  </template>
                </el-dropdown>
                <el-button type="warning" style="margin-left:5px;" icon="ele-MostlyCloudy" @click="importDataRef.openDialog()" v-auth="'stoStockIn:import'"> 导入 </el-button>
              </el-button-group>
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
    </el-card>
    <el-card class="full-table" shadow="hover" style="margin-top: 5px">
      <el-table :data="state.tableData" @selection-change="(val: any[]) => { state.selectData = val; }" style="width: 100%" v-loading="state.tableLoading" tooltip-effect="light" row-key="id" @sort-change="sortChange" border>
        <el-table-column type="selection" width="40" align="center" v-if="auth('stoStockIn:batchDelete') || auth('stoStockIn:export')" />
        <el-table-column type="index" label="序号" width="55" align="center"/>
        <el-table-column prop='stockInNo' label='入库单号' show-overflow-tooltip />
        <el-table-column prop='stockInType' label='入库类型' show-overflow-tooltip>
          <template #default="scope">
            {{ scope.row.stockInType === 'Purchase' ? '采购入库' : scope.row.stockInType === 'SaleReturn' ? '销退入库' : scope.row.stockInType === 'Transfer' ? '调拨入库' : scope.row.stockInType === 'Other' ? '其他入库' : scope.row.stockInType }}
          </template>
        </el-table-column>
        <el-table-column prop='warehouseId' label='入库仓库ID' show-overflow-tooltip v-if="false" />
        <el-table-column prop='warehouseCode' label='仓库编码' show-overflow-tooltip />
        <el-table-column prop='warehouseName' label='仓库名称' show-overflow-tooltip />
        <el-table-column prop='sourceBillNo' label='来源单据号' show-overflow-tooltip />
        <el-table-column prop='stockInDate' label='入库日期' show-overflow-tooltip />
        <el-table-column prop='totalQuantity' label='入库总数量' show-overflow-tooltip />
        <el-table-column prop='totalAmount' label='入库总金额' show-overflow-tooltip />
        <el-table-column prop='status' label='状态' show-overflow-tooltip>
          <template #default="scope">
            {{ scope.row.status === 'Draft' ? '草稿' : scope.row.status === 'Approved' ? '已审批' : scope.row.status === 'Confirmed' ? '已确认' : scope.row.status === 'Cancelled' ? '已取消' : scope.row.status }}
          </template>
        </el-table-column>
        <el-table-column prop='approvalUserId' label='审批人ID' show-overflow-tooltip v-if="false" />
        <el-table-column prop='approvalTime' label='审批时间' show-overflow-tooltip />
        <el-table-column prop='approvalRemark' label='审批意见' show-overflow-tooltip />
        <el-table-column prop='remark' label='备注' show-overflow-tooltip />
        <el-table-column label="修改记录" width="100" align="center" show-overflow-tooltip>
          <template #default="scope">
            <ModifyRecord :data="scope.row" />
          </template>
        </el-table-column>
        <el-table-column label="操作" width="280" align="center" fixed="right" show-overflow-tooltip v-if="auth('stoStockIn:update') || auth('stoStockIn:delete')">
          <template #default="scope">
            <el-button v-if="scope.row.status === 'Draft' || scope.row.status === 'Approved'" icon="ele-Check" size="small" text type="success" @click="confirmStockIn(scope.row)" v-auth="'stoStockIn:update'"> 确认 </el-button>
            <el-button v-if="scope.row.status === 'Confirmed'" icon="ele-RefreshLeft" size="small" text type="warning" @click="cancelConfirmStockIn(scope.row)" v-auth="'stoStockIn:update'"> 取消确认 </el-button>
            <el-button icon="ele-Edit" size="small" text type="primary" @click="editDialogRef.openDialog(scope.row, '编辑入库单主表')" v-auth="'stoStockIn:update'"> 编辑 </el-button>
            <el-button icon="ele-Delete" size="small" text type="primary" @click="delStoStockIn(scope.row)" v-auth="'stoStockIn:delete'"> 删除 </el-button>
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
      <ImportData ref="importDataRef" :import="stoStockInApi.importData" :download="stoStockInApi.downloadTemplate" v-auth="'stoStockIn:import'" @refresh="handleQuery"/>
      <printDialog ref="printDialogRef" :title="'打印入库单主表'" @reloadTable="handleQuery" />
      <editDialog ref="editDialogRef" @reloadTable="handleQuery" />
      <mockStockInDialog ref="mockStockInDialogRef" @reloadTable="handleQuery" />
    </el-card>
  </div>
</template>
<style scoped>
:deep(.el-input), :deep(.el-select), :deep(.el-input-number) {
  width: 100%;
}
</style>