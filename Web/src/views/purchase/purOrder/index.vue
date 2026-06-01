
<script lang="ts" setup name="purOrder">
import { ref, reactive, onMounted } from "vue";
import { auth } from '/@/utils/authFunction';
import { ElMessageBox, ElMessage } from "element-plus";
import { downloadStreamFile } from "/@/utils/download";
import { usePurOrderApi } from '/@/api/purchase/purOrder';
import editDialog from '/@/views/purchase/purOrder/component/editDialog.vue'
import printDialog from '/@/views/system/print/component/hiprint/preview.vue'
import ModifyRecord from '/@/components/table/modifyRecord.vue';
import ImportData from "/@/components/table/importData.vue";

const purOrderApi = usePurOrderApi();
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
    field: 'id',
    order: 'descending',
    descStr: 'descending',
  },
  tableData: [],
  summaryData: null as any,
  showSummaryDialog: false,
});

const statusOptions = [
  { label: '草稿', value: 0 },
  { label: '审批中', value: 1 },
  { label: '已确认', value: 2 },
  { label: '已发货', value: 3 },
  { label: '已入库', value: 4 },
  { label: '已完成', value: 5 },
  { label: '已取消', value: 6 },
];

const getStatusText = (status: number) => {
  const statusMap: Record<number, string> = {
    0: '草稿',
    1: '审批中',
    2: '已确认',
    3: '已发货',
    4: '已入库',
    5: '已完成',
    6: '已取消'
  };
  return statusMap[status] || '未知';
};

const getStatusType = (status: number) => {
  const typeMap: Record<number, string> = {
    0: 'warning',
    1: 'primary',
    2: 'info',
    3: 'info',
    4: 'info',
    5: 'success',
    6: 'danger'
  };
  return typeMap[status] || 'info';
};

onMounted(async () => {
});

const handleQuery = async (params: any = {}) => {
  state.tableLoading = true;
  state.tableParams = Object.assign(state.tableParams, params);
  const result = await purOrderApi.page(Object.assign(state.tableQueryParams, state.tableParams)).then(res => res.data.result);
  state.tableParams.total = result?.total;
  state.tableData = result?.items ?? [];
  state.tableLoading = false;
};

const sortChange = async (column: any) => {
  state.tableParams.field = column.prop;
  state.tableParams.order = column.order;
  await handleQuery();
};

const delPurOrder = (row: any) => {
  ElMessageBox.confirm(`确定要删除采购订单【${row.orderNo}】吗?`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  }).then(async () => {
    await purOrderApi.delete({ id: row.id });
    handleQuery();
    ElMessage.success("删除成功");
  }).catch(() => {});
};

const approvePurOrder = (row: any) => {
  ElMessageBox.confirm(`确定要审核采购订单【${row.orderNo}】吗？`, '审核采购订单', {
    confirmButtonText: '确定',
    cancelButtonText: '取消',
    type: 'warning',
  }).then(async () => {
    await purOrderApi.approve({ id: row.id });
    ElMessage.success('审核成功！');
    handleQuery();
  }).catch(() => {});
};

const approvePurOrderBatch = () => {
  const draftCount = state.selectData.filter(item => item.status === 0).length;
  if (draftCount !== state.selectData.length) {
    ElMessage.warning('只能审核草稿状态的采购订单！');
    return;
  }
  
  ElMessageBox.confirm(`确定要审核选中的${state.selectData.length}条采购订单吗？`, '提示', {
    confirmButtonText: '确定',
    cancelButtonText: '取消',
    type: 'warning',
  }).then(async () => {
    try {
      for (const item of state.selectData) {
        await purOrderApi.approve({ id: item.id });
      }
      ElMessage.success('审核成功！');
      handleQuery();
    } catch (error) {
      ElMessage.error('审核失败！');
    }
  }).catch(() => {});
};

const rejectPurOrder = (row: any) => {
  ElMessageBox.prompt('请输入拒绝原因', '拒绝采购订单', {
    confirmButtonText: '确定',
    cancelButtonText: '取消',
    inputErrorMessage: '拒绝原因不能为空',
  }).then(async (result: any) => {
    await purOrderApi.reject({ 
      id: row.id,
      rejectReason: result.value
    });
    ElMessage.success('已拒绝！');
    handleQuery();
  }).catch(() => {});
};

const rejectPurOrderBatch = () => {
  const draftCount = state.selectData.filter(item => item.status === 0).length;
  if (draftCount !== state.selectData.length) {
    ElMessage.warning('只能拒绝草稿状态的采购订单！');
    return;
  }
  
  ElMessageBox.prompt('请输入拒绝原因', '批量拒绝采购订单', {
    confirmButtonText: '确定',
    cancelButtonText: '取消',
    inputErrorMessage: '拒绝原因不能为空',
  }).then(async (result: any) => {
    try {
      for (const item of state.selectData) {
        await purOrderApi.reject({ 
          id: item.id,
          rejectReason: result.value
        });
      }
      ElMessage.success('拒绝成功！');
      handleQuery();
    } catch (error) {
      ElMessage.error('拒绝失败！');
    }
  }).catch(() => {});
};

const viewSummary = async (row: any) => {
  try {
    const result = await purOrderApi.calculateSummary({ orderId: row.id });
    state.summaryData = result.data?.result || result.data || result;
    state.showSummaryDialog = true;
  } catch (error) {
    ElMessage.error('获取汇总失败！');
  }
};

const batchDelPurOrder = () => {
  ElMessageBox.confirm(`确定要删除${state.selectData.length}条记录吗?`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  }).then(async () => {
    await purOrderApi.batchDelete(state.selectData.map((u: any) => ({ id: u.id }) )).then(res => {
      ElMessage.success(`成功批量删除${res.data.result}条记录`);
      handleQuery();
    });
  }).catch(() => {});
};

const exportPurOrderCommand = async (command: string) => {
  try {
    state.exportLoading = true;
    if (command === 'select') {
      const params = Object.assign({}, state.tableQueryParams, state.tableParams, { selectKeyList: state.selectData.map((u: any) => u.id) });
      await purOrderApi.exportData(params).then(res => downloadStreamFile(res));
    } else if (command === 'current') {
      const params = Object.assign({}, state.tableQueryParams, state.tableParams);
      await purOrderApi.exportData(params).then(res => downloadStreamFile(res));
    } else if (command === 'all') {
      const params = Object.assign({}, state.tableQueryParams, state.tableParams, { page: 1, pageSize: 99999999 });
      await purOrderApi.exportData(params).then(res => downloadStreamFile(res));
    }
  } finally {
    state.exportLoading = false;
  }
}

handleQuery();
</script>
<template>
  <div class="purOrder-container" v-loading="state.exportLoading">
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
            <el-form-item label="供应商名称">
              <el-input v-model="state.tableQueryParams.supplierName" clearable placeholder="请输入供应商名称"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="下单日期">
              <el-date-picker type="daterange" v-model="state.tableQueryParams.orderDateRange"  value-format="YYYY-MM-DD HH:mm:ss" start-placeholder="开始日期" end-placeholder="结束日期" :default-time="[new Date('1 00:00:00'), new Date('1 23:59:59')]" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="状态">
              <el-select v-model="state.tableQueryParams.status" clearable placeholder="请选择状态">
                <el-option v-for="item in statusOptions" :key="item.value" :label="item.label" :value="item.value" />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10">
            <el-form-item>
              <el-button-group style="display: flex; align-items: center;">
                <el-button type="primary" icon="ele-Search" @click="handleQuery" v-auth="'purOrder:page'" v-reclick="1000"> 查询 </el-button>
                <el-button icon="ele-Refresh" @click="() => state.tableQueryParams = {}"> 重置 </el-button>
                <el-button icon="ele-ZoomIn" @click="() => state.showAdvanceQueryUI = true" v-if="!state.showAdvanceQueryUI" style="margin-left:5px;"> 高级查询 </el-button>
                <el-button icon="ele-ZoomOut" @click="() => state.showAdvanceQueryUI = false" v-if="state.showAdvanceQueryUI" style="margin-left:5px;"> 隐藏 </el-button>
                <el-button type="danger" style="margin-left:5px;" icon="ele-Delete" @click="batchDelPurOrder" :disabled="state.selectData.length == 0" v-auth="'purOrder:batchDelete'"> 删除 </el-button>
                <el-button type="success" style="margin-left:5px;" icon="ele-Check" @click="approvePurOrderBatch" :disabled="state.selectData.length == 0"> 审核 </el-button>
                <el-button type="warning" style="margin-left:5px;" icon="ele-X" @click="rejectPurOrderBatch" :disabled="state.selectData.length == 0"> 拒绝审核 </el-button>
                <el-button type="primary" style="margin-left:5px;" icon="ele-Plus" @click="editDialogRef.openDialog(null, '新增采购订单')" v-auth="'purOrder:add'"> 新增 </el-button>
                <el-dropdown :show-timeout="70" :hide-timeout="50" @command="exportPurOrderCommand">
                  <el-button type="primary" style="margin-left:5px;" icon="ele-FolderOpened" v-reclick="20000" v-auth="'purOrder:export'"> 导出 </el-button>
                  <template #dropdown>
                    <el-dropdown-menu>
                      <el-dropdown-item command="select" :disabled="state.selectData.length == 0">导出选中</el-dropdown-item>
                      <el-dropdown-item command="current">导出本页</el-dropdown-item>
                      <el-dropdown-item command="all">导出全部</el-dropdown-item>
                    </el-dropdown-menu>
                  </template>
                </el-dropdown>
                <el-button type="warning" style="margin-left:5px;" icon="ele-MostlyCloudy" @click="importDataRef.openDialog()" v-auth="'purOrder:import'"> 导入 </el-button>
              </el-button-group>
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
    </el-card>
    <el-card class="full-table" shadow="hover" style="margin-top: 5px">
      <el-table :data="state.tableData" @selection-change="(val: any[]) => { state.selectData = val; }" style="width: 100%" v-loading="state.tableLoading" tooltip-effect="light" row-key="id" @sort-change="sortChange" border>
        <el-table-column type="selection" width="40" align="center" v-if="auth('purOrder:batchDelete') || auth('purOrder:export')" />
        <el-table-column type="index" label="序号" width="55" align="center"/>
        <el-table-column prop='orderNo' label='订单号' show-overflow-tooltip />
        <el-table-column prop='supplierCode' label='供应商编码' show-overflow-tooltip />
        <el-table-column prop='supplierName' label='供应商名称' show-overflow-tooltip />
        <el-table-column prop='contractNo' label='合同编号' show-overflow-tooltip />
        <el-table-column prop='orderDate' label='下单日期' show-overflow-tooltip />
        <el-table-column prop='deliveryDate' label='交货日期' show-overflow-tooltip />
        <el-table-column prop='totalQty' label='总数量' show-overflow-tooltip />
        <el-table-column prop='grandTotal' label='价税合计' show-overflow-tooltip />
        <el-table-column prop='status' label='状态' width="80" align="center">
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
        <el-table-column label="操作" width="180" align="center" fixed="right" show-overflow-tooltip>
          <template #default="scope">
            <el-button icon="ele-Edit" size="small" text type="primary" @click="editDialogRef.openDialog(scope.row, '编辑采购订单')" :disabled="scope.row.status !== 0"> 编辑 </el-button>
            <el-button icon="ele-Delete" size="small" text type="danger" @click="delPurOrder(scope.row)" :disabled="scope.row.status !== 0"> 删除 </el-button>
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
      <ImportData ref="importDataRef" :import="purOrderApi.importData" :download="purOrderApi.downloadTemplate" v-auth="'purOrder:import'" @refresh="handleQuery"/>
      <printDialog ref="printDialogRef" :title="'打印采购订单'" @reloadTable="handleQuery" />
      <editDialog ref="editDialogRef" @reloadTable="handleQuery" />
    </el-card>

    <el-dialog
      title="采购订单明细汇总"
      v-model="state.showSummaryDialog"
      width="1000px"
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
                <span class="label">明细数量：</span>
                <span class="value">{{ state.summaryData.itemCount }} 项</span>
              </div>
            </el-col>
            <el-col :span="6">
              <div class="summary-item">
                <span class="label">总数量：</span>
                <span class="value">{{ state.summaryData.totalQty }}</span>
              </div>
            </el-col>
            <el-col :span="6">
              <div class="summary-item">
                <span class="label">价税合计：</span>
                <span class="value">{{ state.summaryData.grandTotal?.toFixed(2) || '0.00' }}</span>
              </div>
            </el-col>
          </el-row>
        </el-card>

        <el-card class="mb10" body-style="{ padding: '10px' }">
          <el-row :gutter="20">
            <el-col :span="6">
              <div class="stat-card">
                <div class="stat-label">总数量</div>
                <div class="stat-value">{{ state.summaryData.totalQty }}</div>
              </div>
            </el-col>
            <el-col :span="6">
              <div class="stat-card">
                <div class="stat-label">总金额(不含税)</div>
                <div class="stat-value">{{ state.summaryData.totalAmount?.toFixed(2) || '0.00' }}</div>
              </div>
            </el-col>
            <el-col :span="6">
              <div class="stat-card">
                <div class="stat-label">税额</div>
                <div class="stat-value">{{ state.summaryData.taxAmount?.toFixed(2) || '0.00' }}</div>
              </div>
            </el-col>
            <el-col :span="6">
              <div class="stat-card grand">
                <div class="stat-label">价税合计</div>
                <div class="stat-value">{{ state.summaryData.grandTotal?.toFixed(2) || '0.00' }}</div>
              </div>
            </el-col>
          </el-row>
        </el-card>

        <el-card body-style="{ padding: '10px' }">
          <el-table :data="state.summaryData.items" border style="width: 100%">
            <el-table-column prop="materialCode" label="物料编码" show-overflow-tooltip />
            <el-table-column prop="materialName" label="物料名称" show-overflow-tooltip />
            <el-table-column prop="spec" label="规格型号" show-overflow-tooltip />
            <el-table-column prop="unitName" label="单位" show-overflow-tooltip />
            <el-table-column prop="orderQty" label="订单数量" align="right" />
            <el-table-column prop="price" label="单价" align="right">
              <template #default="scope">
                {{ scope.row.price?.toFixed(2) || '' }}
              </template>
            </el-table-column>
            <el-table-column prop="taxRate" label="税率(%)" align="right">
              <template #default="scope">
                {{ (scope.row.taxRate || 0) + '%' }}
              </template>
            </el-table-column>
            <el-table-column prop="amount" label="金额" align="right">
              <template #default="scope">
                {{ scope.row.amount?.toFixed(2) || '0.00' }}
              </template>
            </el-table-column>
            <el-table-column prop="taxAmount" label="税额" align="right">
              <template #default="scope">
                {{ scope.row.taxAmount?.toFixed(2) || '0.00' }}
              </template>
            </el-table-column>
            <el-table-column prop="remark" label="备注" show-overflow-tooltip />
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

.summary-container .stat-card {
  padding: 15px;
  border-radius: 8px;
  background: #f5f7fa;
  text-align: center;
}

.summary-container .stat-card.grand {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
}

.summary-container .stat-card .stat-label {
  font-size: 12px;
  color: #909399;
  margin-bottom: 8px;
}

.summary-container .stat-card.grand .stat-label {
  color: rgba(255, 255, 255, 0.9);
}

.summary-container .stat-card .stat-value {
  font-size: 20px;
  font-weight: bold;
  color: #409eff;
}

.summary-container .stat-card.grand .stat-value {
  color: white;
}
</style>

