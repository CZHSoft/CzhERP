<script lang="ts" setup name="stoAdjust">
import { ref, reactive, onMounted } from "vue";
import { auth } from '/@/utils/authFunction';
import { ElMessageBox, ElMessage } from "element-plus";
import { downloadStreamFile } from "/@/utils/download";
import { useStoAdjustApi } from '/@/api/stock/stoAdjust';
import editDialog from '/@/views/stock/stoAdjust/component/editDialog.vue'
import printDialog from '/@/views/system/print/component/hiprint/preview.vue'
import ModifyRecord from '/@/components/table/modifyRecord.vue';
import ImportData from "/@/components/table/importData.vue";

const stoAdjustApi = useStoAdjustApi();
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
});

const getAdjustTypeText = (adjustType: string) => {
  const typeMap: Record<string, string> = {
    'Adjust': '调整',
    'CountDiff': '盘点差异'
  };
  return typeMap[adjustType] || adjustType;
};

const getAdjustTypeTagType = (adjustType: string) => {
  const typeMap: Record<string, string> = {
    'Adjust': 'primary',
    'CountDiff': 'warning'
  };
  return typeMap[adjustType] || 'info';
};

const getStatusText = (status: string) => {
  const statusMap: Record<string, string> = {
    'Draft': '草稿',
    'Approved': '已审核',
    'Completed': '已完成',
    'Cancelled': '已取消'
  };
  return statusMap[status] || status;
};

const getStatusType = (status: string) => {
  const typeMap: Record<string, string> = {
    'Draft': 'warning',
    'Approved': 'success',
    'Completed': 'primary',
    'Cancelled': 'danger'
  };
  return typeMap[status] || 'info';
};

onMounted(async () => {
});

// 查询操作
const handleQuery = async (params: any = {}) => {
  state.tableLoading = true;
  state.tableParams = Object.assign(state.tableParams, params);
  const result = await stoAdjustApi.page(Object.assign(state.tableQueryParams, state.tableParams)).then(res => res.data.result);
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

// 删除
const delStoAdjust = (row: any) => {
  ElMessageBox.confirm(`确定要删除调整单【${row.adjustNo}】吗?`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  }).then(async () => {
    await stoAdjustApi.delete({ id: row.id });
    handleQuery();
    ElMessage.success("删除成功");
  }).catch(() => {});
};

const approveStoAdjust = (row: any) => {
  ElMessageBox.confirm(`确定要审核调整单【${row.adjustNo}】吗？\n审核通过后将执行库存调整！`, "审核调整单", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  }).then(async () => {
    const res = await stoAdjustApi.approve({ id: row.id });
    ElMessage.success(res.data.result || '审核成功！');
    handleQuery();
  }).catch(() => {});
};

const rejectStoAdjust = (row: any) => {
  ElMessageBox.prompt('请输入拒绝原因', '拒绝调整单', {
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
    const res = await stoAdjustApi.reject({ 
      id: row.id,
      rejectReason: value
    });
    ElMessage.success(res.data.result || '已拒绝！');
    handleQuery();
  }).catch(() => {});
};

// 批量删除
const batchDelStoAdjust = () => {
  ElMessageBox.confirm(`确定要删除${state.selectData.length}条记录吗?`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  }).then(async () => {
    await stoAdjustApi.batchDelete(state.selectData.map(u => ({ id: u.id }) )).then(res => {
      ElMessage.success(`成功批量删除${res.data.result}条记录`);
      handleQuery();
    });
  }).catch(() => {});
};

// 导出数据
const exportStoAdjustCommand = async (command: string) => {
  try {
    state.exportLoading = true;
    if (command === 'select') {
      const params = Object.assign({}, state.tableQueryParams, state.tableParams, { selectKeyList: state.selectData.map(u => u.id) });
      await stoAdjustApi.exportData(params).then(res => downloadStreamFile(res));
    } else if (command === 'current') {
      const params = Object.assign({}, state.tableQueryParams, state.tableParams);
      await stoAdjustApi.exportData(params).then(res => downloadStreamFile(res));
    } else if (command === 'all') {
      const params = Object.assign({}, state.tableQueryParams, state.tableParams, { page: 1, pageSize: 99999999 });
      await stoAdjustApi.exportData(params).then(res => downloadStreamFile(res));
    }
  } finally {
    state.exportLoading = false;
  }
}

handleQuery();
</script>
<template>
  <div class="stoAdjust-container" v-loading="state.exportLoading">
    <el-card shadow="hover" :body-style="{ paddingBottom: '0' }"> 
      <el-form :model="state.tableQueryParams" ref="queryForm" labelWidth="90">
        <el-row>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10">
            <el-form-item label="关键字">
              <el-input v-model="state.tableQueryParams.keyword" clearable placeholder="请输入模糊查询关键字"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="调整单号">
              <el-input v-model="state.tableQueryParams.adjustNo" clearable placeholder="请输入调整单号"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="调整类型(Adjust调整/CountDiff盘点差异)">
              <el-input v-model="state.tableQueryParams.adjustType" clearable placeholder="请输入调整类型(Adjust调整/CountDiff盘点差异)"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="仓库ID">
              <el-input v-model="state.tableQueryParams.warehouseId" clearable placeholder="请输入仓库ID"/>
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
            <el-form-item label="调整日期">
              <el-date-picker type="daterange" v-model="state.tableQueryParams.adjustDateRange"  value-format="YYYY-MM-DD HH:mm:ss" start-placeholder="开始日期" end-placeholder="结束日期" :default-time="[new Date('1 00:00:00'), new Date('1 23:59:59')]" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="调整原因">
              <el-input v-model="state.tableQueryParams.adjustReason" clearable placeholder="请输入调整原因"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="调整总数量">
              <el-input-number v-model="state.tableQueryParams.totalQuantity"  clearable placeholder="请输入调整总数量"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="调整总金额">
              <el-input-number v-model="state.tableQueryParams.totalAmount"  clearable placeholder="请输入调整总金额"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="状态(Draft草稿/Approved已审批/Completed已完成/Cancelled已取消)">
              <el-input v-model="state.tableQueryParams.status" clearable placeholder="请输入状态(Draft草稿/Approved已审批/Completed已完成/Cancelled已取消)"/>
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
                <el-button type="primary"  icon="ele-Search" @click="handleQuery" v-auth="'stoAdjust:page'" v-reclick="1000"> 查询 </el-button>
                <el-button icon="ele-Refresh" @click="() => state.tableQueryParams = {}"> 重置 </el-button>
                <el-button icon="ele-ZoomIn" @click="() => state.showAdvanceQueryUI = true" v-if="!state.showAdvanceQueryUI" style="margin-left:5px;"> 高级查询 </el-button>
                <el-button icon="ele-ZoomOut" @click="() => state.showAdvanceQueryUI = false" v-if="state.showAdvanceQueryUI" style="margin-left:5px;"> 隐藏 </el-button>
                <el-button type="danger" style="margin-left:5px;" icon="ele-Delete" @click="batchDelStoAdjust" :disabled="state.selectData.length == 0" v-auth="'stoAdjust:batchDelete'"> 删除 </el-button>
                <el-button type="primary" style="margin-left:5px;" icon="ele-Plus" @click="editDialogRef.openDialog(null, '新增调整单主表')" v-auth="'stoAdjust:add'"> 新增 </el-button>
                <el-dropdown :show-timeout="70" :hide-timeout="50" @command="exportStoAdjustCommand">
                  <el-button type="primary" style="margin-left:5px;" icon="ele-FolderOpened" v-reclick="20000" v-auth="'stoAdjust:export'"> 导出 </el-button>
                  <template #dropdown>
                    <el-dropdown-menu>
                      <el-dropdown-item command="select" :disabled="state.selectData.length == 0">导出选中</el-dropdown-item>
                      <el-dropdown-item command="current">导出本页</el-dropdown-item>
                      <el-dropdown-item command="all">导出全部</el-dropdown-item>
                    </el-dropdown-menu>
                  </template>
                </el-dropdown>
                <el-button type="warning" style="margin-left:5px;" icon="ele-MostlyCloudy" @click="importDataRef.openDialog()" v-auth="'stoAdjust:import'"> 导入 </el-button>
              </el-button-group>
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
    </el-card>
    <el-card class="full-table" shadow="hover" style="margin-top: 5px">
      <el-table :data="state.tableData" @selection-change="(val: any[]) => { state.selectData = val; }" style="width: 100%" v-loading="state.tableLoading" tooltip-effect="light" row-key="id" @sort-change="sortChange" border>
        <el-table-column type="selection" width="40" align="center" v-if="auth('stoAdjust:batchDelete') || auth('stoAdjust:export')" />
        <el-table-column type="index" label="序号" width="55" align="center"/>
        <el-table-column prop='adjustNo' label='调整单号' show-overflow-tooltip />
        <el-table-column prop='adjustType' label='调整类型' width="100" align="center">
          <template #default="scope">
            <el-tag :type="getAdjustTypeTagType(scope.row.adjustType)" size="small">
              {{ getAdjustTypeText(scope.row.adjustType) }}
            </el-tag>
          </template>
        </el-table-column>
        <el-table-column prop='warehouseCode' label='仓库编码' show-overflow-tooltip />
        <el-table-column prop='warehouseName' label='仓库名称' show-overflow-tooltip />
        <el-table-column prop='adjustDate' label='调整日期' show-overflow-tooltip />
        <el-table-column prop='adjustReason' label='调整原因' show-overflow-tooltip />
        <el-table-column prop='totalQuantity' label='调整总数量' show-overflow-tooltip />
        <el-table-column prop='totalAmount' label='调整总金额' show-overflow-tooltip />
        <el-table-column prop='status' label='状态' width="100" align="center">
          <template #default="scope">
            <el-tag :type="getStatusType(scope.row.status)" size="small">
              {{ getStatusText(scope.row.status) }}
            </el-tag>
          </template>
        </el-table-column>
        <el-table-column prop='remark' label='备注' show-overflow-tooltip />
        <el-table-column label="修改记录" width="100" align="center" show-overflow-tooltip>
          <template #default="scope">
            <ModifyRecord :data="scope.row" />
          </template>
        </el-table-column>
        <el-table-column label="操作" width="220" align="center" fixed="right" show-overflow-tooltip>
          <template #default="scope">
            <el-button v-if="scope.row.status === 'Draft'" type="success" size="small" text icon="ele-Check" @click="approveStoAdjust(scope.row)"> 审核 </el-button>
            <el-button v-if="scope.row.status === 'Draft'" type="danger" size="small" text icon="ele-X" @click="rejectStoAdjust(scope.row)"> 拒绝 </el-button>
            <el-button icon="ele-Edit" size="small" text type="primary" @click="editDialogRef.openDialog(scope.row, '编辑调整单')" :disabled="scope.row.status !== 'Draft'"> 编辑 </el-button>
            <el-button icon="ele-Delete" size="small" text type="danger" @click="delStoAdjust(scope.row)" :disabled="scope.row.status !== 'Draft'"> 删除 </el-button>
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
      <ImportData ref="importDataRef" :import="stoAdjustApi.importData" :download="stoAdjustApi.downloadTemplate" v-auth="'stoAdjust:import'" @refresh="handleQuery"/>
      <printDialog ref="printDialogRef" :title="'打印调整单主表'" @reloadTable="handleQuery" />
      <editDialog ref="editDialogRef" @reloadTable="handleQuery" />
    </el-card>
  </div>
</template>
<style scoped>
:deep(.el-input), :deep(.el-select), :deep(.el-input-number) {
  width: 100%;
}
</style>