<script lang="ts" setup name="finPayment">
import { ref, reactive, onMounted } from "vue";
import { auth } from '/@/utils/authFunction';
import { ElMessageBox, ElMessage } from "element-plus";
import { downloadStreamFile } from "/@/utils/download";
import { useFinPaymentApi } from '/@/api/finance/finPayment';
import editDialog from '/@/views/finance/finPayment/component/editDialog.vue'
import printDialog from '/@/views/system/print/component/hiprint/preview.vue'
import ModifyRecord from '/@/components/table/modifyRecord.vue';
import ImportData from "/@/components/table/importData.vue";

const finPaymentApi = useFinPaymentApi();
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
  const result = await finPaymentApi.page(Object.assign(state.tableQueryParams, state.tableParams)).then(res => res.data.result);
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
const delFinPayment = (row: any) => {
  ElMessageBox.confirm(`确定要删除吗?`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  }).then(async () => {
    await finPaymentApi.delete({ id: row.id });
    handleQuery();
    ElMessage.success("删除成功");
  }).catch(() => {});
};

// 批量删除
const batchDelFinPayment = () => {
  ElMessageBox.confirm(`确定要删除${state.selectData.length}条记录吗?`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  }).then(async () => {
    await finPaymentApi.batchDelete(state.selectData.map(u => ({ id: u.id }) )).then(res => {
      ElMessage.success(`成功批量删除${res.data.result}条记录`);
      handleQuery();
    });
  }).catch(() => {});
};

// 审核
const approveFinPayment = (row: any) => {
  ElMessageBox.prompt('请输入审核意见（可选）', '审核确认', {
    confirmButtonText: '确定',
    cancelButtonText: '取消',
    inputPlaceholder: '请输入审核意见',
    inputType: 'textarea',
  }).then(async ({ value }) => {
    state.approveLoading = true;
    try {
      await finPaymentApi.approve({ id: row.id, approvalRemark: value });
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
const rejectFinPayment = (row: any) => {
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
      await finPaymentApi.reject({ id: row.id, rejectReason: value });
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
const exportFinPaymentCommand = async (command: string) => {
  try {
    state.exportLoading = true;
    if (command === 'select') {
      const params = Object.assign({}, state.tableQueryParams, state.tableParams, { selectKeyList: state.selectData.map(u => u.id) });
      await finPaymentApi.exportData(params).then(res => downloadStreamFile(res));
    } else if (command === 'current') {
      const params = Object.assign({}, state.tableQueryParams, state.tableParams);
      await finPaymentApi.exportData(params).then(res => downloadStreamFile(res));
    } else if (command === 'all') {
      const params = Object.assign({}, state.tableQueryParams, state.tableParams, { page: 1, pageSize: 99999999 });
      await finPaymentApi.exportData(params).then(res => downloadStreamFile(res));
    }
  } finally {
    state.exportLoading = false;
  }
}

handleQuery();
</script>
<template>
  <div class="finPayment-container" v-loading="state.exportLoading">
    <el-card shadow="hover" :body-style="{ paddingBottom: '0' }"> 
      <el-form :model="state.tableQueryParams" ref="queryForm" labelWidth="90">
        <el-row>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10">
            <el-form-item label="关键字">
              <el-input v-model="state.tableQueryParams.keyword" clearable placeholder="请输入模糊查询关键字"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="付款单号">
              <el-input v-model="state.tableQueryParams.paymentNo" clearable placeholder="请输入付款单号"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="供应商ID">
              <el-input v-model="state.tableQueryParams.supplierId" clearable placeholder="请输入供应商ID"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="供应商名称">
              <el-input v-model="state.tableQueryParams.supplierName" clearable placeholder="请输入供应商名称"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="付款日期">
              <el-date-picker type="daterange" v-model="state.tableQueryParams.paymentDateRange"  value-format="YYYY-MM-DD HH:mm:ss" start-placeholder="开始日期" end-placeholder="结束日期" :default-time="[new Date('1 00:00:00'), new Date('1 23:59:59')]" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="付款类型">
              <el-input v-model="state.tableQueryParams.paymentType" clearable placeholder="请输入付款类型"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="付款银行账户ID">
              <el-input v-model="state.tableQueryParams.bankAccountId" clearable placeholder="请输入付款银行账户ID"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="付款银行账户">
              <el-input v-model="state.tableQueryParams.bankAccountName" clearable placeholder="请输入付款银行账户"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="付款金额">
              <el-input-number v-model="state.tableQueryParams.paymentAmount"  clearable placeholder="请输入付款金额"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="已核销金额">
              <el-input-number v-model="state.tableQueryParams.paidAmount"  clearable placeholder="请输入已核销金额"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="未核销金额">
              <el-input-number v-model="state.tableQueryParams.unverifyAmount"  clearable placeholder="请输入未核销金额"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="核销单号">
              <el-input v-model="state.tableQueryParams.againstNo" clearable placeholder="请输入核销单号"/>
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
              <el-input v-model="state.tableQueryParams.approverRemark" clearable placeholder="请输入审批意见"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="付款方式">
              <el-input v-model="state.tableQueryParams.paymentMethod" clearable placeholder="请输入付款方式"/>
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
                <el-button type="primary"  icon="ele-Search" @click="handleQuery" v-auth="'finPayment:page'" v-reclick="1000"> 查询 </el-button>
                <el-button icon="ele-Refresh" @click="() => state.tableQueryParams = {}"> 重置 </el-button>
                <el-button icon="ele-ZoomIn" @click="() => state.showAdvanceQueryUI = true" v-if="!state.showAdvanceQueryUI" style="margin-left:5px;"> 高级查询 </el-button>
                <el-button icon="ele-ZoomOut" @click="() => state.showAdvanceQueryUI = false" v-if="state.showAdvanceQueryUI" style="margin-left:5px;"> 隐藏 </el-button>
                <el-button type="danger" style="margin-left:5px;" icon="ele-Delete" @click="batchDelFinPayment" :disabled="state.selectData.length == 0" v-auth="'finPayment:batchDelete'"> 删除 </el-button>
                <el-button type="primary" style="margin-left:5px;" icon="ele-Plus" @click="editDialogRef.openDialog(null, '新增付款记录表')" v-auth="'finPayment:add'"> 新增 </el-button>
                <el-dropdown :show-timeout="70" :hide-timeout="50" @command="exportFinPaymentCommand">
                  <el-button type="primary" style="margin-left:5px;" icon="ele-FolderOpened" v-reclick="20000" v-auth="'finPayment:export'"> 导出 </el-button>
                  <template #dropdown>
                    <el-dropdown-menu>
                      <el-dropdown-item command="select" :disabled="state.selectData.length == 0">导出选中</el-dropdown-item>
                      <el-dropdown-item command="current">导出本页</el-dropdown-item>
                      <el-dropdown-item command="all">导出全部</el-dropdown-item>
                    </el-dropdown-menu>
                  </template>
                </el-dropdown>
                <el-button type="warning" style="margin-left:5px;" icon="ele-MostlyCloudy" @click="importDataRef.openDialog()" v-auth="'finPayment:import'"> 导入 </el-button>
              </el-button-group>
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
    </el-card>
    <el-card class="full-table" shadow="hover" style="margin-top: 5px">
      <el-table :data="state.tableData" @selection-change="(val: any[]) => { state.selectData = val; }" style="width: 100%" v-loading="state.tableLoading" tooltip-effect="light" row-key="id" @sort-change="sortChange" border>
        <el-table-column type="selection" width="40" align="center" v-if="auth('finPayment:batchDelete') || auth('finPayment:export')" />
        <el-table-column type="index" label="序号" width="55" align="center"/>
        <el-table-column prop='paymentNo' label='付款单号' show-overflow-tooltip />
        <el-table-column prop='supplierId' label='供应商ID' show-overflow-tooltip />
        <el-table-column prop='supplierName' label='供应商名称' show-overflow-tooltip />
        <el-table-column prop='paymentDate' label='付款日期' show-overflow-tooltip />
        <el-table-column prop='paymentType' label='付款类型' show-overflow-tooltip />
        <el-table-column prop='bankAccountId' label='付款银行账户ID' show-overflow-tooltip />
        <el-table-column prop='bankAccountName' label='付款银行账户' show-overflow-tooltip />
        <el-table-column prop='paymentAmount' label='付款金额' show-overflow-tooltip />
        <el-table-column prop='paidAmount' label='已核销金额' show-overflow-tooltip />
        <el-table-column prop='unverifyAmount' label='未核销金额' show-overflow-tooltip />
        <el-table-column prop='againstNo' label='核销单号' show-overflow-tooltip />
        <el-table-column prop='status' label='状态' show-overflow-tooltip />
        <el-table-column prop='approvalUserId' label='审批人ID' show-overflow-tooltip />
        <el-table-column prop='approvalTime' label='审批时间' show-overflow-tooltip />
        <el-table-column prop='approverRemark' label='审批意见' show-overflow-tooltip />
        <el-table-column prop='paymentMethod' label='付款方式' show-overflow-tooltip />
        <el-table-column prop='remark' label='备注' show-overflow-tooltip />
        <el-table-column label="修改记录" width="100" align="center" show-overflow-tooltip>
          <template #default="scope">
            <ModifyRecord :data="scope.row" />
          </template>
        </el-table-column>
        <el-table-column label="操作" width="220" align="center" fixed="right" show-overflow-tooltip>
          <template #default="scope">
            <el-button icon="ele-Edit" size="small" text type="primary" @click="editDialogRef.openDialog(scope.row, '编辑付款记录表')"> 编辑 </el-button>
            <el-button icon="ele-Check" size="small" text type="success" @click="approveFinPayment(scope.row)" :loading="state.approveLoading" v-if="scope.row.status === 'Draft' || scope.row.status === 'Pending'"> 审核 </el-button>
            <el-button icon="ele-Close" size="small" text type="danger" @click="rejectFinPayment(scope.row)" :loading="state.approveLoading" v-if="scope.row.status === 'Draft' || scope.row.status === 'Pending'"> 拒绝 </el-button>
            <el-button icon="ele-Delete" size="small" text type="primary" @click="delFinPayment(scope.row)"> 删除 </el-button>
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
      <ImportData ref="importDataRef" :import="finPaymentApi.importData" :download="finPaymentApi.downloadTemplate" v-auth="'finPayment:import'" @refresh="handleQuery"/>
      <printDialog ref="printDialogRef" :title="'打印付款记录表'" @reloadTable="handleQuery" />
      <editDialog ref="editDialogRef" @reloadTable="handleQuery" />
    </el-card>
  </div>
</template>
<style scoped>
:deep(.el-input), :deep(.el-select), :deep(.el-input-number) {
  width: 100%;
}
</style>