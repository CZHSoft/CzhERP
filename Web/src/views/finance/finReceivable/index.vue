<script lang="ts" setup name="finReceivable">
import { ref, reactive, onMounted } from "vue";
import { auth } from '/@/utils/authFunction';
import { ElMessageBox, ElMessage } from "element-plus";
import { downloadStreamFile } from "/@/utils/download";
import { useFinReceivableApi } from '/@/api/finance/finReceivable';
import editDialog from '/@/views/finance/finReceivable/component/editDialog.vue'
import printDialog from '/@/views/system/print/component/hiprint/preview.vue'
import ModifyRecord from '/@/components/table/modifyRecord.vue';
import ImportData from "/@/components/table/importData.vue";

const finReceivableApi = useFinReceivableApi();
const printDialogRef = ref();
const editDialogRef = ref();
const importDataRef = ref();
const state = reactive({
  exportLoading: false,
  tableLoading: false,
  approveLoading: false,
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
  const result = await finReceivableApi.page(Object.assign(state.tableQueryParams, state.tableParams)).then(res => res.data.result);
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
const delFinReceivable = (row: any) => {
  ElMessageBox.confirm(`确定要删除吗?`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  }).then(async () => {
    await finReceivableApi.delete({ id: row.id });
    handleQuery();
    ElMessage.success("删除成功");
  }).catch(() => {});
};

// 批量删除
const batchDelFinReceivable = () => {
  ElMessageBox.confirm(`确定要删除${state.selectData.length}条记录吗?`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  }).then(async () => {
    await finReceivableApi.batchDelete(state.selectData.map(u => ({ id: u.id }) )).then(res => {
      ElMessage.success(`成功批量删除${res.data.result}条记录`);
      handleQuery();
    });
  }).catch(() => {});
};

// 审核
const approveFinReceivable = (row: any) => {
  ElMessageBox.prompt('请输入审核意见（可选）', '审核确认', {
    confirmButtonText: '确定',
    cancelButtonText: '取消',
    inputPlaceholder: '请输入审核意见',
    inputType: 'textarea',
  }).then(async ({ value }) => {
    state.approveLoading = true;
    try {
      await finReceivableApi.approve({ id: row.id, approvalRemark: value });
      ElMessage.success("审核成功");
      handleQuery();
    } catch (error) {
      ElMessage.error("审核失败");
    } finally {
      state.approveLoading = false;
    }
  }).catch(() => {});
};

// 拒绝
const rejectFinReceivable = (row: any) => {
  ElMessageBox.prompt('请输入拒绝原因', '拒绝确认', {
    confirmButtonText: '确定',
    cancelButtonText: '取消',
    inputPlaceholder: '请输入拒绝原因',
    inputType: 'textarea',
    inputValidator: (value) => {
      if (!value || value.trim() === '') {
        return '拒绝原因不能为空';
      }
      return true;
    },
  }).then(async ({ value }) => {
    state.approveLoading = true;
    try {
      await finReceivableApi.reject({ id: row.id, rejectReason: value });
      ElMessage.success("拒绝成功");
      handleQuery();
    } catch (error) {
      ElMessage.error("拒绝失败");
    } finally {
      state.approveLoading = false;
    }
  }).catch(() => {});
};

// 导出数据
const exportFinReceivableCommand = async (command: string) => {
  try {
    state.exportLoading = true;
    if (command === 'select') {
      const params = Object.assign({}, state.tableQueryParams, state.tableParams, { selectKeyList: state.selectData.map(u => u.id) });
      await finReceivableApi.exportData(params).then(res => downloadStreamFile(res));
    } else if (command === 'current') {
      const params = Object.assign({}, state.tableQueryParams, state.tableParams);
      await finReceivableApi.exportData(params).then(res => downloadStreamFile(res));
    } else if (command === 'all') {
      const params = Object.assign({}, state.tableQueryParams, state.tableParams, { page: 1, pageSize: 99999999 });
      await finReceivableApi.exportData(params).then(res => downloadStreamFile(res));
    }
  } finally {
    state.exportLoading = false;
  }
}

handleQuery();
</script>
<template>
  <div class="finReceivable-container" v-loading="state.exportLoading">
    <el-card shadow="hover" :body-style="{ paddingBottom: '0' }"> 
      <el-form :model="state.tableQueryParams" ref="queryForm" labelWidth="90">
        <el-row>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10">
            <el-form-item label="关键字">
              <el-input v-model="state.tableQueryParams.keyword" clearable placeholder="请输入模糊查询关键字"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="应收单号">
              <el-input v-model="state.tableQueryParams.receivableNo" clearable placeholder="请输入应收单号"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="客户ID">
              <el-input v-model="state.tableQueryParams.customerId" clearable placeholder="请输入客户ID"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="客户编码">
              <el-input v-model="state.tableQueryParams.customerCode" clearable placeholder="请输入客户编码"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="客户名称">
              <el-input v-model="state.tableQueryParams.customerName" clearable placeholder="请输入客户名称"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="来源单据类型">
              <el-input v-model="state.tableQueryParams.sourceType" clearable placeholder="请输入来源单据类型"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="来源单据ID">
              <el-input v-model="state.tableQueryParams.sourceId" clearable placeholder="请输入来源单据ID"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="来源单据号">
              <el-input v-model="state.tableQueryParams.sourceNo" clearable placeholder="请输入来源单据号"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="单据日期">
              <el-date-picker type="daterange" v-model="state.tableQueryParams.billDateRange"  value-format="YYYY-MM-DD HH:mm:ss" start-placeholder="开始日期" end-placeholder="结束日期" :default-time="[new Date('1 00:00:00'), new Date('1 23:59:59')]" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="到期日期">
              <el-date-picker type="daterange" v-model="state.tableQueryParams.dueDateRange"  value-format="YYYY-MM-DD HH:mm:ss" start-placeholder="开始日期" end-placeholder="结束日期" :default-time="[new Date('1 00:00:00'), new Date('1 23:59:59')]" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="应收金额">
              <el-input-number v-model="state.tableQueryParams.amount"  clearable placeholder="请输入应收金额"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="已收金额">
              <el-input-number v-model="state.tableQueryParams.receivedAmount"  clearable placeholder="请输入已收金额"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="未收金额">
              <el-input-number v-model="state.tableQueryParams.unreceivedAmount"  clearable placeholder="请输入未收金额"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="逾期天数">
              <el-input-number v-model="state.tableQueryParams.overdueDays"  clearable placeholder="请输入逾期天数"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="状态">
              <el-input v-model="state.tableQueryParams.status" clearable placeholder="请输入状态"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="业务员ID">
              <el-input v-model="state.tableQueryParams.salesmanId" clearable placeholder="请输入业务员ID"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="业务员姓名">
              <el-input v-model="state.tableQueryParams.salesmanName" clearable placeholder="请输入业务员姓名"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="部门ID">
              <el-input v-model="state.tableQueryParams.departmentId" clearable placeholder="请输入部门ID"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="部门名称">
              <el-input v-model="state.tableQueryParams.departmentName" clearable placeholder="请输入部门名称"/>
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
                <el-button type="primary"  icon="ele-Search" @click="handleQuery" v-auth="'finReceivable:page'" v-reclick="1000"> 查询 </el-button>
                <el-button icon="ele-Refresh" @click="() => state.tableQueryParams = {}"> 重置 </el-button>
                <el-button icon="ele-ZoomIn" @click="() => state.showAdvanceQueryUI = true" v-if="!state.showAdvanceQueryUI" style="margin-left:5px;"> 高级查询 </el-button>
                <el-button icon="ele-ZoomOut" @click="() => state.showAdvanceQueryUI = false" v-if="state.showAdvanceQueryUI" style="margin-left:5px;"> 隐藏 </el-button>
                <el-button type="danger" style="margin-left:5px;" icon="ele-Delete" @click="batchDelFinReceivable" :disabled="state.selectData.length == 0" v-auth="'finReceivable:batchDelete'"> 删除 </el-button>
                <el-button type="primary" style="margin-left:5px;" icon="ele-Plus" @click="editDialogRef.openDialog(null, '新增应收账款表')" v-auth="'finReceivable:add'"> 新增 </el-button>
                <el-dropdown :show-timeout="70" :hide-timeout="50" @command="exportFinReceivableCommand">
                  <el-button type="primary" style="margin-left:5px;" icon="ele-FolderOpened" v-reclick="20000" v-auth="'finReceivable:export'"> 导出 </el-button>
                  <template #dropdown>
                    <el-dropdown-menu>
                      <el-dropdown-item command="select" :disabled="state.selectData.length == 0">导出选中</el-dropdown-item>
                      <el-dropdown-item command="current">导出本页</el-dropdown-item>
                      <el-dropdown-item command="all">导出全部</el-dropdown-item>
                    </el-dropdown-menu>
                  </template>
                </el-dropdown>
                <el-button type="warning" style="margin-left:5px;" icon="ele-MostlyCloudy" @click="importDataRef.openDialog()" v-auth="'finReceivable:import'"> 导入 </el-button>
              </el-button-group>
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
    </el-card>
    <el-card class="full-table" shadow="hover" style="margin-top: 5px">
      <el-table :data="state.tableData" @selection-change="(val: any[]) => { state.selectData = val; }" style="width: 100%" v-loading="state.tableLoading" tooltip-effect="light" row-key="id" @sort-change="sortChange" border>
        <el-table-column type="selection" width="40" align="center" v-if="auth('finReceivable:batchDelete') || auth('finReceivable:export')" />
        <el-table-column type="index" label="序号" width="55" align="center"/>
        <el-table-column prop='receivableNo' label='应收单号' show-overflow-tooltip />
        <el-table-column prop='customerId' label='客户ID' show-overflow-tooltip />
        <el-table-column prop='customerCode' label='客户编码' show-overflow-tooltip />
        <el-table-column prop='customerName' label='客户名称' show-overflow-tooltip />
        <el-table-column prop='sourceType' label='来源单据类型' show-overflow-tooltip />
        <el-table-column prop='sourceId' label='来源单据ID' show-overflow-tooltip />
        <el-table-column prop='sourceNo' label='来源单据号' show-overflow-tooltip />
        <el-table-column prop='billDate' label='单据日期' show-overflow-tooltip />
        <el-table-column prop='dueDate' label='到期日期' show-overflow-tooltip />
        <el-table-column prop='amount' label='应收金额' show-overflow-tooltip />
        <el-table-column prop='receivedAmount' label='已收金额' show-overflow-tooltip />
        <el-table-column prop='unreceivedAmount' label='未收金额' show-overflow-tooltip />
        <el-table-column prop='overdueDays' label='逾期天数' show-overflow-tooltip />
        <el-table-column prop='status' label='状态' show-overflow-tooltip />
        <el-table-column prop='salesmanId' label='业务员ID' show-overflow-tooltip />
        <el-table-column prop='salesmanName' label='业务员姓名' show-overflow-tooltip />
        <el-table-column prop='departmentId' label='部门ID' show-overflow-tooltip />
        <el-table-column prop='departmentName' label='部门名称' show-overflow-tooltip />
        <el-table-column prop='remark' label='备注' show-overflow-tooltip />
        <el-table-column label="修改记录" width="100" align="center" show-overflow-tooltip>
          <template #default="scope">
            <ModifyRecord :data="scope.row" />
          </template>
        </el-table-column>
        <el-table-column label="操作" width="220" align="center" fixed="right" show-overflow-tooltip>
          <template #default="scope">
            <el-button icon="ele-Edit" size="small" text type="primary" @click="editDialogRef.openDialog(scope.row, '编辑应收账款表')"> 编辑 </el-button>
            <el-button icon="ele-Check" size="small" text type="success" @click="approveFinReceivable(scope.row)" :loading="state.approveLoading" v-if="scope.row.status === 'Pending'"> 审核 </el-button>
            <el-button icon="ele-Close" size="small" text type="danger" @click="rejectFinReceivable(scope.row)" :loading="state.approveLoading" v-if="scope.row.status === 'Pending'"> 拒绝 </el-button>
            <el-button icon="ele-Delete" size="small" text type="primary" @click="delFinReceivable(scope.row)"> 删除 </el-button>
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
      <ImportData ref="importDataRef" :import="finReceivableApi.importData" :download="finReceivableApi.downloadTemplate" v-auth="'finReceivable:import'" @refresh="handleQuery"/>
      <printDialog ref="printDialogRef" :title="'打印应收账款表'" @reloadTable="handleQuery" />
      <editDialog ref="editDialogRef" @reloadTable="handleQuery" />
    </el-card>
  </div>
</template>
<style scoped>
:deep(.el-input), :deep(.el-select), :deep(.el-input-number) {
  width: 100%;
}
</style>