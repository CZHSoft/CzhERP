<script lang="ts" setup name="salOrder">
import { ref, reactive, onMounted } from "vue";
import { auth } from '/@/utils/authFunction';
import { ElMessageBox, ElMessage } from "element-plus";
import { downloadStreamFile } from "/@/utils/download";
import { useSalOrderApi } from '/@/api/sale/salOrder';
import editDialog from '/@/views/sale/salOrder/component/editDialog.vue'
import printDialog from '/@/views/system/print/component/hiprint/preview.vue'
import ModifyRecord from '/@/components/table/modifyRecord.vue';
import ImportData from "/@/components/table/importData.vue";

const salOrderApi = useSalOrderApi();
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
    field: 'createTime',
    order: 'descending',
    descStr: 'descending',
  },
  tableData: [],
  statusOptions: [] as any[],
  summaryData: null as any,
  showSummaryDialog: false,
});

const statusMap: Record<string, string> = {
  'Draft': '草稿',
  'Pending': '待审批',
  'Approved': '已审核',
  'Rejected': '已拒绝',
  'Shipped': '已发货',
  'Completed': '已完成',
  'Cancelled': '已取消'
};

const getStatusLabel = (status: string) => {
  return statusMap[status] || status;
};

const canApprove = (status: string) => {
  return status === 'Draft' || status === 'Pending';
};

// 页面加载时
onMounted(async () => {
  await loadStatusOptions();
});

const loadStatusOptions = async () => {
  try {
    const result = await salOrderApi.getStatusOptions();
    state.statusOptions = result.data?.result || result.data || result;
  } catch (error) {
    console.error('加载状态选项失败:', error);
  }
};

// 查询操作
const handleQuery = async (params: any = {}) => {
  state.tableLoading = true;
  state.tableParams = Object.assign(state.tableParams, params);
  const result = await salOrderApi.page(Object.assign(state.tableQueryParams, state.tableParams)).then(res => res.data.result);
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
const delSalOrder = (row: any) => {
  ElMessageBox.confirm(`确定要删除吗?`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  }).then(async () => {
    await salOrderApi.delete({ id: row.id });
    handleQuery();
    ElMessage.success("删除成功");
  }).catch(() => {});
};

// 批量删除
const batchDelSalOrder = () => {
  ElMessageBox.confirm(`确定要删除${state.selectData.length}条记录吗?`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  }).then(async () => {
    await salOrderApi.batchDelete(state.selectData.map(u => ({ id: u.id }) )).then(res => {
      ElMessage.success(`成功批量删除${res.data.result}条记录`);
      handleQuery();
    });
  }).catch(() => {});
};

// 批量审批订单
const approveSalOrder = () => {
  const draftCount = state.selectData.filter(item => item.status === 'Draft' || item.status === 'Pending').length;
  if (draftCount !== state.selectData.length) {
    ElMessage.warning('只能审批草稿或待审批状态的订单');
    return;
  }
  
  ElMessageBox.confirm(`确定要审批选中的${state.selectData.length}条订单吗?`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  }).then(async () => {
    try {
      for (const item of state.selectData) {
        await salOrderApi.approve({ id: item.id, isApproved: true, approvalRemark: '' });
      }
      ElMessage.success('审批成功');
      handleQuery();
    } catch (error) {
      ElMessage.error('审批失败');
    }
  }).catch(() => {});
};

// 批量拒绝审批订单
const rejectSalOrder = () => {
  const draftCount = state.selectData.filter(item => item.status === 'Draft' || item.status === 'Pending').length;
  if (draftCount !== state.selectData.length) {
    ElMessage.warning('只能拒绝草稿或待审批状态的订单');
    return;
  }
  
  ElMessageBox.confirm(`确定要拒绝选中的${state.selectData.length}条订单吗?`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  }).then(async () => {
    try {
      for (const item of state.selectData) {
        await salOrderApi.approve({ id: item.id, isApproved: false, approvalRemark: '' });
      }
      ElMessage.success('拒绝成功');
      handleQuery();
    } catch (error) {
      ElMessage.error('拒绝失败');
    }
  }).catch(() => {});
};

// 查看订单汇总
const viewSummary = async (row: any) => {
  try {
    const result = await salOrderApi.calculateSummary({ orderId: row.id });
    state.summaryData = result.data?.result || result.data || result;
    state.showSummaryDialog = true;
  } catch (error) {
    ElMessage.error('获取汇总失败');
  }
};

// 导出数据
const exportSalOrderCommand = async (command: string) => {
  try {
    state.exportLoading = true;
    if (command === 'select') {
      const params = Object.assign({}, state.tableQueryParams, state.tableParams, { selectKeyList: state.selectData.map(u => u.id) });
      await salOrderApi.exportData(params).then(res => downloadStreamFile(res));
    } else if (command === 'current') {
      const params = Object.assign({}, state.tableQueryParams, state.tableParams);
      await salOrderApi.exportData(params).then(res => downloadStreamFile(res));
    } else if (command === 'all') {
      const params = Object.assign({}, state.tableQueryParams, state.tableParams, { page: 1, pageSize: 99999999 });
      await salOrderApi.exportData(params).then(res => downloadStreamFile(res));
    }
  } finally {
    state.exportLoading = false;
  }
};

const showApproveDialog = (row: any, isApproved: boolean) => {
  const title = isApproved ? '审核通过' : '拒绝订单';
  const message = isApproved ? `确定要审核通过订单【${row.orderNo}】吗？` : `确定要拒绝订单【${row.orderNo}】吗？`;
  
  ElMessageBox.confirm(message, title, {
    confirmButtonText: '确定',
    cancelButtonText: '取消',
    type: isApproved ? 'success' : 'warning',
    input: !isApproved,
    inputPlaceholder: '请输入拒绝原因',
    inputValidator: (value: string) => {
      if (!isApproved && !value.trim()) {
        return '请输入拒绝原因';
      }
      return true;
    }
  }).then(async (inputValue) => {
    try {
      await salOrderApi.approve({
        id: row.id,
        isApproved: isApproved,
        approvalRemark: typeof inputValue === 'string' ? inputValue : ''
      });
      ElMessage.success(isApproved ? '审核通过成功' : '拒绝成功');
      handleQuery();
    } catch (error: any) {
      ElMessage.error(error.response?.data?.message || (isApproved ? '审核失败' : '拒绝失败'));
    }
  }).catch(() => {});
};

const approveOrder = (row: any) => {
  showApproveDialog(row, true);
};

const rejectOrder = (row: any) => {
  showApproveDialog(row, false);
};

handleQuery();
</script>
<template>
  <div class="salOrder-container" v-loading="state.exportLoading">
    <el-card shadow="hover" :body-style="{ paddingBottom: '0' }"> 
      <el-form :model="state.tableQueryParams" ref="queryForm" labelWidth="90">
        <el-row>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10">
            <el-form-item label="关键字">
              <el-input v-model="state.tableQueryParams.keyword" clearable placeholder="请输入模糊查询关键字"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="订单号">
              <el-input v-model="state.tableQueryParams.orderNo" clearable placeholder="请输入订单号"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="客户ID">
              <el-input v-model="state.tableQueryParams.customerId" clearable placeholder="请输入客户ID"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="客户名称">
              <el-input v-model="state.tableQueryParams.customerName" clearable placeholder="请输入客户名称"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="联系人姓名">
              <el-input v-model="state.tableQueryParams.contactName" clearable placeholder="请输入联系人姓名"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="联系电话">
              <el-input v-model="state.tableQueryParams.contactPhone" clearable placeholder="请输入联系电话"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="下单日期">
              <el-date-picker type="daterange" v-model="state.tableQueryParams.orderDateRange"  value-format="YYYY-MM-DD HH:mm:ss" start-placeholder="开始日期" end-placeholder="结束日期" :default-time="[new Date('1 00:00:00'), new Date('1 23:59:59')]" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="预计交货日期">
              <el-date-picker type="daterange" v-model="state.tableQueryParams.deliveryDateRange"  value-format="YYYY-MM-DD HH:mm:ss" start-placeholder="开始日期" end-placeholder="结束日期" :default-time="[new Date('1 00:00:00'), new Date('1 23:59:59')]" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="送货地址">
              <el-input v-model="state.tableQueryParams.address" clearable placeholder="请输入送货地址"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="配送方式">
              <el-input v-model="state.tableQueryParams.shippingMethod" clearable placeholder="请输入配送方式"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="运费">
              <el-input-number v-model="state.tableQueryParams.shippingFee"  clearable placeholder="请输入运费"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="总金额">
              <el-input-number v-model="state.tableQueryParams.totalAmount"  clearable placeholder="请输入总金额"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="总税额">
              <el-input-number v-model="state.tableQueryParams.totalTaxAmount"  clearable placeholder="请输入总税额"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="总折扣">
              <el-input-number v-model="state.tableQueryParams.totalDiscount"  clearable placeholder="请输入总折扣"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="已付款金额">
              <el-input-number v-model="state.tableQueryParams.payAmount"  clearable placeholder="请输入已付款金额"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="付款方式">
              <el-input v-model="state.tableQueryParams.paymentType" clearable placeholder="请输入付款方式"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="状态">
              <el-input v-model="state.tableQueryParams.status" clearable placeholder="请输入状态"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="信用检查结果">
              <el-input v-model="state.tableQueryParams.creditCheckResult" clearable placeholder="请输入信用检查结果"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="本次使用信用额度">
              <el-input-number v-model="state.tableQueryParams.creditUsedAmount"  clearable placeholder="请输入本次使用信用额度"/>
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
            <el-form-item label="审批备注">
              <el-input v-model="state.tableQueryParams.approvalRemark" clearable placeholder="请输入审批备注"/>
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
                <el-button type="primary"  icon="ele-Search" @click="handleQuery" v-auth="'salOrder:page'" v-reclick="1000"> 查询 </el-button>
                <el-button icon="ele-Refresh" @click="() => state.tableQueryParams = {}"> 重置 </el-button>
                <el-button icon="ele-ZoomIn" @click="() => state.showAdvanceQueryUI = true" v-if="!state.showAdvanceQueryUI" style="margin-left:5px;"> 高级查询 </el-button>
                <el-button icon="ele-ZoomOut" @click="() => state.showAdvanceQueryUI = false" v-if="state.showAdvanceQueryUI" style="margin-left:5px;"> 隐藏 </el-button>
                <el-button type="danger" style="margin-left:5px;" icon="ele-Delete" @click="batchDelSalOrder" :disabled="state.selectData.length == 0" v-auth="'salOrder:batchDelete'"> 删除 </el-button>
                <el-button type="primary" style="margin-left:5px;" icon="ele-Plus" @click="editDialogRef.openDialog(null, '新增销售订单')" v-auth="'salOrder:add'"> 新增 </el-button>
                <el-button type="success" style="margin-left:5px;" icon="ele-Check" @click="approveSalOrder" :disabled="state.selectData.length == 0" > 审批 </el-button>
                <el-button type="warning" style="margin-left:5px;" icon="ele-X" @click="rejectSalOrder" :disabled="state.selectData.length == 0" > 拒绝审批 </el-button>
                <el-dropdown :show-timeout="70" :hide-timeout="50" @command="exportSalOrderCommand">
                  <el-button type="primary" style="margin-left:5px;" icon="ele-FolderOpened" v-reclick="20000" v-auth="'salOrder:export'"> 导出 </el-button>
                  <template #dropdown>
                    <el-dropdown-menu>
                      <el-dropdown-item command="select" :disabled="state.selectData.length == 0">导出选中</el-dropdown-item>
                      <el-dropdown-item command="current">导出本页</el-dropdown-item>
                      <el-dropdown-item command="all">导出全部</el-dropdown-item>
                    </el-dropdown-menu>
                  </template>
                </el-dropdown>
                <el-button type="warning" style="margin-left:5px;" icon="ele-MostlyCloudy" @click="importDataRef.openDialog()" v-auth="'salOrder:import'"> 导入 </el-button>
              </el-button-group>
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
    </el-card>
    <el-card class="full-table" shadow="hover" style="margin-top: 5px">
      <el-table :data="state.tableData" @selection-change="(val: any[]) => { state.selectData = val; }" style="width: 100%" v-loading="state.tableLoading" tooltip-effect="light" row-key="id" @sort-change="sortChange" border>
        <el-table-column type="selection" width="40" align="center" v-if="auth('salOrder:batchDelete') || auth('salOrder:export')" />
        <el-table-column type="index" label="序号" width="55" align="center"/>
        <el-table-column prop='orderNo' label='订单号' show-overflow-tooltip />
        <el-table-column prop='customerId' label='客户ID' show-overflow-tooltip />
        <el-table-column prop='customerName' label='客户名称' show-overflow-tooltip />
        <el-table-column prop='contactName' label='联系人姓名' show-overflow-tooltip />
        <el-table-column prop='contactPhone' label='联系电话' show-overflow-tooltip />
        <el-table-column prop='orderDate' label='下单日期' show-overflow-tooltip />
        <el-table-column prop='deliveryDate' label='预计交货日期' show-overflow-tooltip />
        <el-table-column prop='address' label='送货地址' show-overflow-tooltip />
        <el-table-column prop='shippingMethod' label='配送方式' show-overflow-tooltip />
        <el-table-column prop='shippingFee' label='运费' show-overflow-tooltip />
        <el-table-column prop='totalAmount' label='总金额' show-overflow-tooltip />
        <el-table-column prop='totalTaxAmount' label='总税额' show-overflow-tooltip />
        <el-table-column prop='totalDiscount' label='总折扣' show-overflow-tooltip />
        <el-table-column prop='payAmount' label='已付款金额' show-overflow-tooltip />
        <el-table-column prop='paymentType' label='付款方式' show-overflow-tooltip />
        <el-table-column prop='status' label='状态' show-overflow-tooltip>
          <template #default="scope">
            <el-tag :type="scope.row.status === 'Approved' ? 'success' : scope.row.status === 'Rejected' ? 'danger' : scope.row.status === 'Shipped' ? 'warning' : scope.row.status === 'Completed' ? 'info' : 'primary'">
              {{ getStatusLabel(scope.row.status) }}
            </el-tag>
          </template>
        </el-table-column>
        <el-table-column prop='creditCheckResult' label='信用检查结果' show-overflow-tooltip />
        <el-table-column prop='creditUsedAmount' label='本次使用信用额度' show-overflow-tooltip />
        <el-table-column prop='approvalUserId' label='审批人ID' show-overflow-tooltip />
        <el-table-column prop='approvalTime' label='审批时间' show-overflow-tooltip />
        <el-table-column prop='approvalRemark' label='审批备注' show-overflow-tooltip />
        <el-table-column prop='remark' label='备注' show-overflow-tooltip />
        <el-table-column label="修改记录" width="100" align="center" show-overflow-tooltip>
          <template #default="scope">
            <ModifyRecord :data="scope.row" />
          </template>
        </el-table-column>
        <el-table-column label="操作" width="150" align="center" fixed="right" show-overflow-tooltip v-if="auth('salOrder:update') || auth('salOrder:delete')">
          <template #default="scope">
            <el-button icon="ele-Edit" size="small" text type="primary" @click="editDialogRef.openDialog(scope.row, '编辑销售订单')" v-auth="'salOrder:update'"> 编辑 </el-button>
            <el-button icon="ele-Delete" size="small" text type="primary" @click="delSalOrder(scope.row)" v-auth="'salOrder:delete'"> 删除 </el-button>
            <el-button icon="ele-BarChart3" size="small" text type="primary" @click="viewSummary(scope.row)"> 汇总 </el-button>
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
      <ImportData ref="importDataRef" :import="salOrderApi.importData" :download="salOrderApi.downloadTemplate" v-auth="'salOrder:import'" @refresh="handleQuery"/>
      <printDialog ref="printDialogRef" :title="'打印销售订单'" @reloadTable="handleQuery" />
      <editDialog ref="editDialogRef" @reloadTable="handleQuery" />
    </el-card>

    <el-dialog
      title="订单明细汇总"
      v-model="state.showSummaryDialog"
      width="900px"
      @close="() => { state.showSummaryDialog = false; state.summaryData = null; }"
    >
      <div v-if="state.summaryData" class="summary-container">
        <el-card class="mb10" body-style="{ padding: '10px' }">
          <el-row :gutter="20">
            <el-col :span="6">
              <div class="summary-item">
                <span class="label">订单号：</span>
                <span class="value">{{ state.summaryData.orderNo }}</span>
              </div>
            </el-col>
            <el-col :span="6">
              <div class="summary-item">
                <span class="label">客户名称：</span>
                <span class="value">{{ state.summaryData.customerName }}</span>
              </div>
            </el-col>
            <el-col :span="6">
              <div class="summary-item">
                <span class="label">明细数量：</span>
                <span class="value">{{ state.summaryData.itemCount }} 项</span>
              </div>
            </el-col>
            <el-col :span="6">
              <div class="summary-item">
                <span class="label">总数量：</span>
                <span class="value">{{ state.summaryData.totalQuantity }}</span>
              </div>
            </el-col>
          </el-row>
        </el-card>

        <el-card class="mb10" body-style="{ padding: '10px' }">
          <div class="text-center">
            <div class="total-section">
              <div class="total-label">总金额（不含税）</div>
              <div class="total-value">{{ state.summaryData.totalAmount?.toFixed(2) || '0.00' }}</div>
            </div>
            <div class="total-section">
              <div class="total-label">总税额</div>
              <div class="total-value tax">{{ state.summaryData.totalTaxAmount?.toFixed(2) || '0.00' }}</div>
            </div>
            <div class="total-section grand">
              <div class="total-label">含税总金额</div>
              <div class="total-value">{{ state.summaryData.totalAmountWithTax?.toFixed(2) || '0.00' }}</div>
            </div>
            <div class="total-section">
              <div class="total-label">总折扣金额</div>
              <div class="total-value discount">{{ state.summaryData.totalDiscountAmount?.toFixed(2) || '0.00' }}</div>
            </div>
            <div class="total-section">
              <div class="total-label">平均折扣率</div>
              <div class="total-value discount">{{ ((state.summaryData.totalDiscount || 0) * 100).toFixed(2) }}%</div>
            </div>
          </div>
        </el-card>

        <el-card body-style="{ padding: '10px' }">
          <el-table :data="state.summaryData.items" border style="width: 100%">
            <el-table-column prop="materialCode" label="物料编码" show-overflow-tooltip />
            <el-table-column prop="materialName" label="物料名称" show-overflow-tooltip />
            <el-table-column prop="spec" label="规格" show-overflow-tooltip />
            <el-table-column prop="unit" label="单位" show-overflow-tooltip />
            <el-table-column prop="quantity" label="数量" align="right" />
            <el-table-column prop="unitPrice" label="单价" align="right" />
            <el-table-column prop="discount" label="折扣" align="right">
              <template #default="scope">{{ ((scope.row.discount || 1) * 100).toFixed(0) }}%</template>
            </el-table-column>
            <el-table-column prop="actualUnitPrice" label="实际单价" align="right" />
            <el-table-column prop="amount" label="金额" align="right" />
            <el-table-column prop="taxAmount" label="税额" align="right" />
            <el-table-column prop="totalAmount" label="含税金额" align="right" />
          </el-table>
        </el-card>
      </div>
    </el-dialog>
  </div>
</template>
<style scoped>
:deep(.el-input), :deep(.el-select), :deep(.el-input-number) {
  width: 100%;
}

.summary-container .mb10 {
  margin-bottom: 10px;
}

.summary-container .summary-item {
  padding: 8px 0;
  border-bottom: 1px solid #f0f0f0;
}

.summary-container .summary-item .label {
  color: #909399;
  font-size: 13px;
}

.summary-container .summary-item .value {
  color: #303133;
  font-size: 13px;
  font-weight: 500;
}

.summary-container .text-center {
  text-align: center;
}

.summary-container .total-section {
  display: inline-block;
  padding: 15px 25px;
  margin: 5px;
  border-radius: 8px;
  background: #f5f7fa;
  min-width: 180px;
}

.summary-container .total-section.grand {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
}

.summary-container .total-section .total-label {
  font-size: 13px;
  color: #909399;
  margin-bottom: 8px;
}

.summary-container .total-section.grand .total-label {
  color: rgba(255, 255, 255, 0.9);
}

.summary-container .total-section .total-value {
  font-size: 24px;
  font-weight: bold;
  color: #409eff;
}

.summary-container .total-section.grand .total-value {
  color: white;
}

.summary-container .total-section .total-value.tax {
  color: #e6a23c;
}

.summary-container .total-section .total-value.discount {
  color: #f56c6c;
}

.summary-container .discount-info {
  margin-top: 15px;
  color: #909399;
  font-size: 13px;
}
</style>