<script lang="ts" setup name="stoInventory">
import { ref, reactive, onMounted } from "vue";
import { auth } from '/@/utils/authFunction';
import { ElMessageBox, ElMessage } from "element-plus";
import { downloadStreamFile } from "/@/utils/download";
import { useStoInventoryApi } from '/@/api/stock/stoInventory';
import editDialog from '/@/views/stock/stoInventory/component/editDialog.vue'
import printDialog from '/@/views/system/print/component/hiprint/preview.vue'
import ModifyRecord from '/@/components/table/modifyRecord.vue';
import ImportData from "/@/components/table/importData.vue";

const stoInventoryApi = useStoInventoryApi();
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
  const result = await stoInventoryApi.page(Object.assign(state.tableQueryParams, state.tableParams)).then(res => res.data.result);
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
const delStoInventory = (row: any) => {
  ElMessageBox.confirm(`确定要删除吗?`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  }).then(async () => {
    await stoInventoryApi.delete({ id: row.id });
    handleQuery();
    ElMessage.success("删除成功");
  }).catch(() => {});
};

// 批量删除
const batchDelStoInventory = () => {
  ElMessageBox.confirm(`确定要删除${state.selectData.length}条记录吗?`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  }).then(async () => {
    await stoInventoryApi.batchDelete(state.selectData.map(u => ({ id: u.id }) )).then(res => {
      ElMessage.success(`成功批量删除${res.data.result}条记录`);
      handleQuery();
    });
  }).catch(() => {});
};

// 导出数据
const exportStoInventoryCommand = async (command: string) => {
  try {
    state.exportLoading = true;
    if (command === 'select') {
      const params = Object.assign({}, state.tableQueryParams, state.tableParams, { selectKeyList: state.selectData.map(u => u.id) });
      await stoInventoryApi.exportData(params).then(res => downloadStreamFile(res));
    } else if (command === 'current') {
      const params = Object.assign({}, state.tableQueryParams, state.tableParams);
      await stoInventoryApi.exportData(params).then(res => downloadStreamFile(res));
    } else if (command === 'all') {
      const params = Object.assign({}, state.tableQueryParams, state.tableParams, { page: 1, pageSize: 99999999 });
      await stoInventoryApi.exportData(params).then(res => downloadStreamFile(res));
    }
  } finally {
    state.exportLoading = false;
  }
}

handleQuery();
</script>
<template>
  <div class="stoInventory-container" v-loading="state.exportLoading">
    <el-card shadow="hover" :body-style="{ paddingBottom: '0' }"> 
      <el-form :model="state.tableQueryParams" ref="queryForm" labelWidth="90">
        <el-row>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10">
            <el-form-item label="关键字">
              <el-input v-model="state.tableQueryParams.keyword" clearable placeholder="请输入模糊查询关键字"/>
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
            <el-form-item label="物料ID">
              <el-input v-model="state.tableQueryParams.materialId" clearable placeholder="请输入物料ID"/>
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
            <el-form-item label="单位">
              <el-input v-model="state.tableQueryParams.unit" clearable placeholder="请输入单位"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="库存数量">
              <el-input-number v-model="state.tableQueryParams.stockQuantity"  clearable placeholder="请输入库存数量"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="冻结数量">
              <el-input-number v-model="state.tableQueryParams.frozenQuantity"  clearable placeholder="请输入冻结数量"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="可用数量">
              <el-input-number v-model="state.tableQueryParams.availableQuantity"  clearable placeholder="请输入可用数量"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="成本单价">
              <el-input-number v-model="state.tableQueryParams.costPrice"  clearable placeholder="请输入成本单价"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="库存总成本">
              <el-input-number v-model="state.tableQueryParams.totalCost"  clearable placeholder="请输入库存总成本"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="最低库存">
              <el-input-number v-model="state.tableQueryParams.minStock"  clearable placeholder="请输入最低库存"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="最高库存">
              <el-input-number v-model="state.tableQueryParams.maxStock"  clearable placeholder="请输入最高库存"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10">
            <el-form-item >
              <el-button-group style="display: flex; align-items: center;">
                <el-button type="primary"  icon="ele-Search" @click="handleQuery" v-auth="'stoInventory:page'" v-reclick="1000"> 查询 </el-button>
                <el-button icon="ele-Refresh" @click="() => state.tableQueryParams = {}"> 重置 </el-button>
                <el-button icon="ele-ZoomIn" @click="() => state.showAdvanceQueryUI = true" v-if="!state.showAdvanceQueryUI" style="margin-left:5px;"> 高级查询 </el-button>
                <el-button icon="ele-ZoomOut" @click="() => state.showAdvanceQueryUI = false" v-if="state.showAdvanceQueryUI" style="margin-left:5px;"> 隐藏 </el-button>
                <el-button type="danger" style="margin-left:5px;" icon="ele-Delete" @click="batchDelStoInventory" :disabled="state.selectData.length == 0" v-auth="'stoInventory:batchDelete'"> 删除 </el-button>
                <el-button type="primary" style="margin-left:5px;" icon="ele-Plus" @click="editDialogRef.openDialog(null, '新增库存余额')" v-auth="'stoInventory:add'"> 新增 </el-button>
                <el-dropdown :show-timeout="70" :hide-timeout="50" @command="exportStoInventoryCommand">
                  <el-button type="primary" style="margin-left:5px;" icon="ele-FolderOpened" v-reclick="20000" v-auth="'stoInventory:export'"> 导出 </el-button>
                  <template #dropdown>
                    <el-dropdown-menu>
                      <el-dropdown-item command="select" :disabled="state.selectData.length == 0">导出选中</el-dropdown-item>
                      <el-dropdown-item command="current">导出本页</el-dropdown-item>
                      <el-dropdown-item command="all">导出全部</el-dropdown-item>
                    </el-dropdown-menu>
                  </template>
                </el-dropdown>
                <el-button type="warning" style="margin-left:5px;" icon="ele-MostlyCloudy" @click="importDataRef.openDialog()" v-auth="'stoInventory:import'"> 导入 </el-button>
              </el-button-group>
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
    </el-card>
    <el-card class="full-table" shadow="hover" style="margin-top: 5px">
      <el-table :data="state.tableData" @selection-change="(val: any[]) => { state.selectData = val; }" style="width: 100%" v-loading="state.tableLoading" tooltip-effect="light" row-key="id" @sort-change="sortChange" border>
        <el-table-column type="selection" width="40" align="center" v-if="auth('stoInventory:batchDelete') || auth('stoInventory:export')" />
        <el-table-column type="index" label="序号" width="55" align="center"/>
        <el-table-column prop='warehouseId' label='仓库ID' show-overflow-tooltip />
        <el-table-column prop='warehouseCode' label='仓库编码' show-overflow-tooltip />
        <el-table-column prop='warehouseName' label='仓库名称' show-overflow-tooltip />
        <el-table-column prop='materialId' label='物料ID' show-overflow-tooltip />
        <el-table-column prop='materialCode' label='物料编码' show-overflow-tooltip />
        <el-table-column prop='materialName' label='物料名称' show-overflow-tooltip />
        <el-table-column prop='spec' label='规格型号' show-overflow-tooltip />
        <el-table-column prop='unit' label='单位' show-overflow-tooltip />
        <el-table-column prop='stockQuantity' label='库存数量' show-overflow-tooltip />
        <el-table-column prop='frozenQuantity' label='冻结数量' show-overflow-tooltip />
        <el-table-column prop='availableQuantity' label='可用数量' show-overflow-tooltip />
        <el-table-column prop='costPrice' label='成本单价' show-overflow-tooltip />
        <el-table-column prop='totalCost' label='库存总成本' show-overflow-tooltip />
        <el-table-column prop='minStock' label='最低库存' show-overflow-tooltip />
        <el-table-column prop='maxStock' label='最高库存' show-overflow-tooltip />
        <el-table-column label="修改记录" width="100" align="center" show-overflow-tooltip>
          <template #default="scope">
            <ModifyRecord :data="scope.row" />
          </template>
        </el-table-column>
        <el-table-column label="操作" width="140" align="center" fixed="right" show-overflow-tooltip v-if="auth('stoInventory:update') || auth('stoInventory:delete')">
          <template #default="scope">
            <el-button icon="ele-Edit" size="small" text type="primary" @click="editDialogRef.openDialog(scope.row, '编辑库存余额')" v-auth="'stoInventory:update'"> 编辑 </el-button>
            <el-button icon="ele-Delete" size="small" text type="primary" @click="delStoInventory(scope.row)" v-auth="'stoInventory:delete'"> 删除 </el-button>
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
      <ImportData ref="importDataRef" :import="stoInventoryApi.importData" :download="stoInventoryApi.downloadTemplate" v-auth="'stoInventory:import'" @refresh="handleQuery"/>
      <printDialog ref="printDialogRef" :title="'打印库存余额'" @reloadTable="handleQuery" />
      <editDialog ref="editDialogRef" @reloadTable="handleQuery" />
    </el-card>
  </div>
</template>
<style scoped>
:deep(.el-input), :deep(.el-select), :deep(.el-input-number) {
  width: 100%;
}
</style>