<script lang="ts" setup name="stoStockOutItem">
import { ref, reactive, onMounted } from "vue";
import { auth } from '/@/utils/authFunction';
import { ElMessageBox, ElMessage } from "element-plus";
import { downloadStreamFile } from "/@/utils/download";
import { useStoStockOutItemApi } from '/@/api/stock/stoStockOutItem';
import editDialog from '/@/views/stock/stoStockOutItem/component/editDialog.vue'
import printDialog from '/@/views/system/print/component/hiprint/preview.vue'
import ModifyRecord from '/@/components/table/modifyRecord.vue';
import ImportData from "/@/components/table/importData.vue";

const stoStockOutItemApi = useStoStockOutItemApi();
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

onMounted(async () => {
});

// 查询操作
const handleQuery = async (params: any = {}) => {
  state.tableLoading = true;
  state.tableParams = Object.assign(state.tableParams, params);
  const result = await stoStockOutItemApi.page(Object.assign(state.tableQueryParams, state.tableParams)).then(res => res.data.result);
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
const delStoStockOutItem = (row: any) => {
  ElMessageBox.confirm(`确定要删除出库明细【${row.materialCode} - ${row.materialName}】吗?`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  }).then(async () => {
    await stoStockOutItemApi.delete({ id: row.id });
    handleQuery();
    ElMessage.success("删除成功");
  }).catch(() => {});
};

// 批量删除
const batchDelStoStockOutItem = () => {
  ElMessageBox.confirm(`确定要删除${state.selectData.length}条记录吗?`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  }).then(async () => {
    await stoStockOutItemApi.batchDelete(state.selectData.map(u => ({ id: u.id })));
    ElMessage.success(`成功批量删除${state.selectData.length}条记录`);
    handleQuery();
  }).catch(() => {});
};

// 导出数据
const exportStoStockOutItemCommand = async (command: string) => {
  try {
    state.exportLoading = true;
    if (command === 'select') {
      const params = Object.assign({}, state.tableQueryParams, state.tableParams, { selectKeyList: state.selectData.map(u => u.id) });
      await stoStockOutItemApi.exportData(params).then(res => downloadStreamFile(res));
    } else if (command === 'current') {
      const params = Object.assign({}, state.tableQueryParams, state.tableParams);
      await stoStockOutItemApi.exportData(params).then(res => downloadStreamFile(res));
    } else if (command === 'all') {
      const params = Object.assign({}, state.tableQueryParams, state.tableParams, { page: 1, pageSize: 99999999 });
      await stoStockOutItemApi.exportData(params).then(res => downloadStreamFile(res));
    }
  } finally {
    state.exportLoading = false;
  }
}

handleQuery();
</script>
<template>
  <div class="stoStockOutItem-container" v-loading="state.exportLoading">
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
            <el-form-item label="物料编码">
              <el-input v-model="state.tableQueryParams.materialCode" clearable placeholder="请输入物料编码"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="物料名称">
              <el-input v-model="state.tableQueryParams.materialName" clearable placeholder="请输入物料名称"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="规格型号">
              <el-input v-model="state.tableQueryParams.spec" clearable placeholder="请输入规格型号"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="批号">
              <el-input v-model="state.tableQueryParams.batchNo" clearable placeholder="请输入批号"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="库位">
              <el-input v-model="state.tableQueryParams.locationCode" clearable placeholder="请输入库位"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10">
            <el-form-item >
              <el-button-group style="display: flex; align-items: center;">
                <el-button type="primary" icon="ele-Search" @click="handleQuery" v-auth="'stoStockOutItem:page'" v-reclick="1000"> 查询 </el-button>
                <el-button icon="ele-Refresh" @click="() => state.tableQueryParams = {}"> 重置 </el-button>
                <el-button icon="ele-ZoomIn" @click="() => state.showAdvanceQueryUI = true" v-if="!state.showAdvanceQueryUI" style="margin-left:5px;"> 高级查询 </el-button>
                <el-button icon="ele-ZoomOut" @click="() => state.showAdvanceQueryUI = false" v-if="state.showAdvanceQueryUI" style="margin-left:5px;"> 隐藏 </el-button>
              </el-button-group>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb10">
            <el-form-item >
              <el-button-group style="display: flex; align-items: center;">
                <el-button type="primary" icon="ele-Plus" @click="editDialogRef.openDialog(null, '新增出库明细')" v-auth="'stoStockOutItem:add'" v-reclick="1000"> 新增 </el-button>
                <el-button type="danger" icon="ele-Delete" @click="batchDelStoStockOutItem" :disabled="state.selectData.length == 0" v-auth="'stoStockOutItem:batchDelete'" style="margin-left:5px;"> 删除 </el-button>
                <el-dropdown :show-timeout="70" :hide-timeout="50" @command="exportStoStockOutItemCommand" style="margin-left:5px;">
                  <el-button type="primary" icon="ele-FolderOpened" v-reclick="20000" v-auth="'stoStockOutItem:export'"> 导出 </el-button>
                  <template #dropdown>
                    <el-dropdown-menu>
                      <el-dropdown-item command="select" :disabled="state.selectData.length == 0">导出选中</el-dropdown-item>
                      <el-dropdown-item command="current">导出本页</el-dropdown-item>
                      <el-dropdown-item command="all">导出全部</el-dropdown-item>
                    </el-dropdown-menu>
                  </template>
                </el-dropdown>
                <el-button type="warning" icon="ele-Upload" @click="importDataRef.openDialog()" v-auth="'stoStockOutItem:import'" style="margin-left:5px;"> 导入 </el-button>
              </el-button-group>
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
    </el-card>
    <el-card class="full-table" shadow="hover" style="margin-top: 5px">
      <el-table :data="state.tableData" @selection-change="(val: any[]) => { state.selectData = val; }" style="width: 100%" v-loading="state.tableLoading" tooltip-effect="light" row-key="id" @sort-change="sortChange" border>
        <el-table-column type="selection" width="40" align="center" v-if="auth('stoStockOutItem:batchDelete') || auth('stoStockOutItem:export')" />
        <el-table-column type="index" label="序号" width="55" align="center"/>
        <el-table-column prop="stockOutId" label="出库单ID" width="150" show-overflow-tooltip />
        <el-table-column prop="materialCode" label="物料编码" width="120" show-overflow-tooltip />
        <el-table-column prop="materialName" label="物料名称" width="150" show-overflow-tooltip />
        <el-table-column prop="spec" label="规格" width="120" show-overflow-tooltip />
        <el-table-column prop="unit" label="单位" width="60" align="center" show-overflow-tooltip />
        <el-table-column prop="quantity" label="数量" width="80" align="right" show-overflow-tooltip />
        <el-table-column prop="unitPrice" label="单价" width="80" align="right" show-overflow-tooltip>
          <template #default="scope">
            {{ scope.row.unitPrice?.toFixed(2) }}
          </template>
        </el-table-column>
        <el-table-column prop="amount" label="金额" width="100" align="right" show-overflow-tooltip>
          <template #default="scope">
            {{ scope.row.amount?.toFixed(2) }}
          </template>
        </el-table-column>
        <el-table-column prop="locationCode" label="库位" width="100" show-overflow-tooltip />
        <el-table-column prop="batchNo" label="批号" width="120" show-overflow-tooltip />
        <el-table-column prop="remark" label="备注" show-overflow-tooltip />
        <el-table-column label="修改记录" width="80" align="center" show-overflow-tooltip>
          <template #default="scope">
            <ModifyRecord :data="scope.row" />
          </template>
        </el-table-column>
        <el-table-column label="操作" width="140" align="center" fixed="right" show-overflow-tooltip v-if="auth('stoStockOutItem:update') || auth('stoStockOutItem:delete')">
          <template #default="scope">
            <el-button icon="ele-Edit" size="small" text type="primary" @click="editDialogRef.openDialog(scope.row, '编辑出库明细')" v-auth="'stoStockOutItem:update'"> 编辑 </el-button>
            <el-button icon="ele-Delete" size="small" text type="danger" @click="delStoStockOutItem(scope.row)" v-auth="'stoStockOutItem:delete'"> 删除 </el-button>
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
      <ImportData ref="importDataRef" :import="stoStockOutItemApi.importData" :download="stoStockOutItemApi.downloadTemplate" v-auth="'stoStockOutItem:import'" @refresh="handleQuery"/>
      <printDialog ref="printDialogRef" :title="'打印出库明细'" @reloadTable="handleQuery" />
      <editDialog ref="editDialogRef" @reloadTable="handleQuery" />
    </el-card>
  </div>
</template>
<style scoped>
:deep(.el-input), :deep(.el-select), :deep(.el-input-number) {
  width: 100%;
}
</style>