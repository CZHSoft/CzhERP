<script lang="ts" setup name="finVoucherDetail">
import { ref, reactive, onMounted } from "vue";
import { auth } from '/@/utils/authFunction';
import { ElMessageBox, ElMessage } from "element-plus";
import { downloadStreamFile } from "/@/utils/download";
import { useFinVoucherDetailApi } from '/@/api/finance/finVoucherDetail';
import editDialog from '/@/views/finance/finVoucherDetail/component/editDialog.vue'
import printDialog from '/@/views/system/print/component/hiprint/preview.vue'
import ModifyRecord from '/@/components/table/modifyRecord.vue';
import ImportData from "/@/components/table/importData.vue";

const finVoucherDetailApi = useFinVoucherDetailApi();
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
		field: '', // 默认的排序字段
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
  const result = await finVoucherDetailApi.page(Object.assign(state.tableQueryParams, state.tableParams)).then(res => res.data.result);
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
const delFinVoucherDetail = (row: any) => {
  ElMessageBox.confirm(`确定要删除吗?`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  }).then(async () => {
    await finVoucherDetailApi.delete({ id: row.id });
    handleQuery();
    ElMessage.success("删除成功");
  }).catch(() => {});
};

// 批量删除
const batchDelFinVoucherDetail = () => {
  ElMessageBox.confirm(`确定要删除${state.selectData.length}条记录吗?`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  }).then(async () => {
    await finVoucherDetailApi.batchDelete(state.selectData.map(u => ({ id: u.id }) )).then(res => {
      ElMessage.success(`成功批量删除${res.data.result}条记录`);
      handleQuery();
    });
  }).catch(() => {});
};

// 导出数据
const exportFinVoucherDetailCommand = async (command: string) => {
  try {
    state.exportLoading = true;
    if (command === 'select') {
      const params = Object.assign({}, state.tableQueryParams, state.tableParams, { selectKeyList: state.selectData.map(u => u.id) });
      await finVoucherDetailApi.exportData(params).then(res => downloadStreamFile(res));
    } else if (command === 'current') {
      const params = Object.assign({}, state.tableQueryParams, state.tableParams);
      await finVoucherDetailApi.exportData(params).then(res => downloadStreamFile(res));
    } else if (command === 'all') {
      const params = Object.assign({}, state.tableQueryParams, state.tableParams, { page: 1, pageSize: 99999999 });
      await finVoucherDetailApi.exportData(params).then(res => downloadStreamFile(res));
    }
  } finally {
    state.exportLoading = false;
  }
}

handleQuery();
</script>
<template>
  <div class="finVoucherDetail-container" v-loading="state.exportLoading">
    <el-card shadow="hover" :body-style="{ paddingBottom: '0' }"> 
      <el-form :model="state.tableQueryParams" ref="queryForm" labelWidth="90">
        <el-row>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10">
            <el-form-item label="关键字">
              <el-input v-model="state.tableQueryParams.keyword" clearable placeholder="请输入模糊查询关键字"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="凭证ID">
              <el-input v-model="state.tableQueryParams.voucherId" clearable placeholder="请输入凭证ID"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="科目ID">
              <el-input v-model="state.tableQueryParams.accountId" clearable placeholder="请输入科目ID"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="科目编码">
              <el-input v-model="state.tableQueryParams.accountCode" clearable placeholder="请输入科目编码"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="科目名称">
              <el-input v-model="state.tableQueryParams.accountName" clearable placeholder="请输入科目名称"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="摘要">
              <el-input v-model="state.tableQueryParams.summary" clearable placeholder="请输入摘要"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="借方金额">
              <el-input-number v-model="state.tableQueryParams.debitAmount"  clearable placeholder="请输入借方金额"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="贷方金额">
              <el-input-number v-model="state.tableQueryParams.creditAmount"  clearable placeholder="请输入贷方金额"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="部门ID">
              <el-input v-model="state.tableQueryParams.deptId" clearable placeholder="请输入部门ID"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="部门名称">
              <el-input v-model="state.tableQueryParams.deptName" clearable placeholder="请输入部门名称"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="个人ID">
              <el-input v-model="state.tableQueryParams.personId" clearable placeholder="请输入个人ID"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="个人姓名">
              <el-input v-model="state.tableQueryParams.personName" clearable placeholder="请输入个人姓名"/>
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
            <el-form-item label="项目ID">
              <el-input v-model="state.tableQueryParams.projectId" clearable placeholder="请输入项目ID"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="项目名称">
              <el-input v-model="state.tableQueryParams.projectName" clearable placeholder="请输入项目名称"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="存货ID">
              <el-input v-model="state.tableQueryParams.inventoryId" clearable placeholder="请输入存货ID"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="存货名称">
              <el-input v-model="state.tableQueryParams.inventoryName" clearable placeholder="请输入存货名称"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="现金流量编码">
              <el-input v-model="state.tableQueryParams.cashFlowCode" clearable placeholder="请输入现金流量编码"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="现金流量名称">
              <el-input v-model="state.tableQueryParams.cashFlowName" clearable placeholder="请输入现金流量名称"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="排序号">
              <el-input-number v-model="state.tableQueryParams.sortOrder"  clearable placeholder="请输入排序号"/>
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
                <el-button type="primary"  icon="ele-Search" @click="handleQuery" v-auth="'finVoucherDetail:page'" v-reclick="1000"> 查询 </el-button>
                <el-button icon="ele-Refresh" @click="() => state.tableQueryParams = {}"> 重置 </el-button>
                <el-button icon="ele-ZoomIn" @click="() => state.showAdvanceQueryUI = true" v-if="!state.showAdvanceQueryUI" style="margin-left:5px;"> 高级查询 </el-button>
                <el-button icon="ele-ZoomOut" @click="() => state.showAdvanceQueryUI = false" v-if="state.showAdvanceQueryUI" style="margin-left:5px;"> 隐藏 </el-button>
                <el-button type="danger" style="margin-left:5px;" icon="ele-Delete" @click="batchDelFinVoucherDetail" :disabled="state.selectData.length == 0" v-auth="'finVoucherDetail:batchDelete'"> 删除 </el-button>
                <el-button type="primary" style="margin-left:5px;" icon="ele-Plus" @click="editDialogRef.openDialog(null, '新增凭证分录表')" v-auth="'finVoucherDetail:add'"> 新增 </el-button>
                <el-dropdown :show-timeout="70" :hide-timeout="50" @command="exportFinVoucherDetailCommand">
                  <el-button type="primary" style="margin-left:5px;" icon="ele-FolderOpened" v-reclick="20000" v-auth="'finVoucherDetail:export'"> 导出 </el-button>
                  <template #dropdown>
                    <el-dropdown-menu>
                      <el-dropdown-item command="select" :disabled="state.selectData.length == 0">导出选中</el-dropdown-item>
                      <el-dropdown-item command="current">导出本页</el-dropdown-item>
                      <el-dropdown-item command="all">导出全部</el-dropdown-item>
                    </el-dropdown-menu>
                  </template>
                </el-dropdown>
                <el-button type="warning" style="margin-left:5px;" icon="ele-MostlyCloudy" @click="importDataRef.openDialog()" v-auth="'finVoucherDetail:import'"> 导入 </el-button>
              </el-button-group>
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
    </el-card>
    <el-card class="full-table" shadow="hover" style="margin-top: 5px">
      <el-table :data="state.tableData" @selection-change="(val: any[]) => { state.selectData = val; }" style="width: 100%" v-loading="state.tableLoading" tooltip-effect="light" row-key="id" @sort-change="sortChange" border>
        <el-table-column type="selection" width="40" align="center" v-if="auth('finVoucherDetail:batchDelete') || auth('finVoucherDetail:export')" />
        <el-table-column type="index" label="序号" width="55" align="center"/>
        <el-table-column prop='voucherId' label='凭证ID' show-overflow-tooltip />
        <el-table-column prop='accountId' label='科目ID' show-overflow-tooltip />
        <el-table-column prop='accountCode' label='科目编码' show-overflow-tooltip />
        <el-table-column prop='accountName' label='科目名称' show-overflow-tooltip />
        <el-table-column prop='summary' label='摘要' show-overflow-tooltip />
        <el-table-column prop='debitAmount' label='借方金额' show-overflow-tooltip />
        <el-table-column prop='creditAmount' label='贷方金额' show-overflow-tooltip />
        <el-table-column prop='deptId' label='部门ID' show-overflow-tooltip />
        <el-table-column prop='deptName' label='部门名称' show-overflow-tooltip />
        <el-table-column prop='personId' label='个人ID' show-overflow-tooltip />
        <el-table-column prop='personName' label='个人姓名' show-overflow-tooltip />
        <el-table-column prop='supplierId' label='供应商ID' show-overflow-tooltip />
        <el-table-column prop='supplierName' label='供应商名称' show-overflow-tooltip />
        <el-table-column prop='customerId' label='客户ID' show-overflow-tooltip />
        <el-table-column prop='customerName' label='客户名称' show-overflow-tooltip />
        <el-table-column prop='projectId' label='项目ID' show-overflow-tooltip />
        <el-table-column prop='projectName' label='项目名称' show-overflow-tooltip />
        <el-table-column prop='inventoryId' label='存货ID' show-overflow-tooltip />
        <el-table-column prop='inventoryName' label='存货名称' show-overflow-tooltip />
        <el-table-column prop='cashFlowCode' label='现金流量编码' show-overflow-tooltip />
        <el-table-column prop='cashFlowName' label='现金流量名称' show-overflow-tooltip />
        <el-table-column prop='sortOrder' label='排序号' show-overflow-tooltip />
        <el-table-column prop='remark' label='备注' show-overflow-tooltip />
        <el-table-column label="修改记录" width="100" align="center" show-overflow-tooltip>
          <template #default="scope">
            <ModifyRecord :data="scope.row" />
          </template>
        </el-table-column>
        <el-table-column label="操作" width="140" align="center" fixed="right" show-overflow-tooltip v-if="auth('finVoucherDetail:update') || auth('finVoucherDetail:delete')">
          <template #default="scope">
            <el-button icon="ele-Edit" size="small" text type="primary" @click="editDialogRef.openDialog(scope.row, '编辑凭证分录表')" v-auth="'finVoucherDetail:update'"> 编辑 </el-button>
            <el-button icon="ele-Delete" size="small" text type="primary" @click="delFinVoucherDetail(scope.row)" v-auth="'finVoucherDetail:delete'"> 删除 </el-button>
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
      <ImportData ref="importDataRef" :import="finVoucherDetailApi.importData" :download="finVoucherDetailApi.downloadTemplate" v-auth="'finVoucherDetail:import'" @refresh="handleQuery"/>
      <printDialog ref="printDialogRef" :title="'打印凭证分录表'" @reloadTable="handleQuery" />
      <editDialog ref="editDialogRef" @reloadTable="handleQuery" />
    </el-card>
  </div>
</template>
<style scoped>
:deep(.el-input), :deep(.el-select), :deep(.el-input-number) {
  width: 100%;
}
</style>