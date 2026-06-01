<script lang="ts" setup name="stoCount">
import { ref, reactive, onMounted } from "vue";
import { auth } from '/@/utils/authFunction';
import { ElMessageBox, ElMessage } from "element-plus";
import { downloadStreamFile } from "/@/utils/download";
import { useStoCountApi } from '/@/api/stock/stoCount';
import { getWarehouseList } from '/@/api/stock/stoWarehouse';
import editDialog from '/@/views/stock/stoCount/component/editDialog.vue'
import printDialog from '/@/views/system/print/component/hiprint/preview.vue'
import ModifyRecord from '/@/components/table/modifyRecord.vue';
import ImportData from "/@/components/table/importData.vue";

const stoCountApi = useStoCountApi();
const printDialogRef = ref();
const editDialogRef = ref();
const importDataRef = ref();
const state = reactive({
  exportLoading: false,
  tableLoading: false,
  stores: {},
  showAdvanceQueryUI: false,
  dropdownData: {
    warehouseList: [] as any[],
  },
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

// 状态选项
const statusOptions = [
  { label: '草稿', value: 'Draft' },
  { label: '盘点中', value: 'Counting' },
  { label: '已完成', value: 'Completed' }
];

// 页面加载时
onMounted(async () => {
  await loadWarehouseList();
});

// 加载仓库列表
const loadWarehouseList = async () => {
  const res = await getWarehouseList();
  state.dropdownData.warehouseList = res.data.result.items || [];
};

// 查询操作
const handleQuery = async (params: any = {}) => {
  state.tableLoading = true;
  state.tableParams = Object.assign(state.tableParams, params);
  const result = await stoCountApi.page(Object.assign(state.tableQueryParams, state.tableParams)).then(res => res.data.result);
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

// 获取状态文本
const getStatusText = (status: string) => {
  const statusMap: Record<string, string> = {
    'Draft': '草稿',
    'Counting': '盘点中',
    'Completed': '已完成'
  };
  return statusMap[status] || status;
};

// 获取状态标签类型
const getStatusType = (status: string) => {
  const typeMap: Record<string, string> = {
    'Draft': 'warning',
    'Counting': 'primary',
    'Completed': 'success'
  };
  return typeMap[status] || 'info';
};

// 删除
const delStoCount = (row: any) => {
  ElMessageBox.confirm(`确定要删除盘点单【${row.countNo}】吗?`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  }).then(async () => {
    await stoCountApi.delete({ id: row.id });
    handleQuery();
    ElMessage.success("删除成功");
  }).catch(() => {});
};

// 审核盘点单
const approveStoCount = (row: any) => {
  ElMessageBox.confirm(`确定要审核盘点单【${row.countNo}】吗？审核通过后将根据盘点结果调整库存！`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  }).then(async () => {
    await stoCountApi.approve({ id: row.id });
    handleQuery();
    ElMessage.success("审核成功");
  }).catch(() => {});
};

// 拒绝盘点单
const rejectDialogVisible = ref(false);
const rejectForm = reactive({
  id: 0,
  countNo: '',
  reason: ''
});

const rejectStoCount = (row: any) => {
  rejectForm.id = row.id;
  rejectForm.countNo = row.countNo;
  rejectForm.reason = '';
  rejectDialogVisible.value = true;
};

const submitReject = async () => {
  if (!rejectForm.reason) {
    ElMessage.warning("请填写拒绝原因");
    return;
  }
  await stoCountApi.reject({ id: rejectForm.id, reason: rejectForm.reason });
  rejectDialogVisible.value = false;
  handleQuery();
  ElMessage.success("已拒绝盘点单");
};

// 批量删除
const batchDelStoCount = () => {
  ElMessageBox.confirm(`确定要删除${state.selectData.length}条记录吗?`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  }).then(async () => {
    await stoCountApi.batchDelete(state.selectData.map(u => ({ id: u.id }) )).then(res => {
      ElMessage.success(`成功批量删除${res.data.result}条记录`);
      handleQuery();
    });
  }).catch(() => {});
};

// 导出数据
const exportStoCountCommand = async (command: string) => {
  try {
    state.exportLoading = true;
    if (command === 'select') {
      const params = Object.assign({}, state.tableQueryParams, state.tableParams, { selectKeyList: state.selectData.map(u => u.id) });
      await stoCountApi.exportData(params).then(res => downloadStreamFile(res));
    } else if (command === 'current') {
      const params = Object.assign({}, state.tableQueryParams, state.tableParams);
      await stoCountApi.exportData(params).then(res => downloadStreamFile(res));
    } else if (command === 'all') {
      const params = Object.assign({}, state.tableQueryParams, state.tableParams, { page: 1, pageSize: 99999999 });
      await stoCountApi.exportData(params).then(res => downloadStreamFile(res));
    }
  } finally {
    state.exportLoading = false;
  }
}

handleQuery();
</script>
<template>
  <div class="stoCount-container" v-loading="state.exportLoading">
    <el-card shadow="hover" :body-style="{ paddingBottom: '0' }"> 
      <el-form :model="state.tableQueryParams" ref="queryForm" labelWidth="90">
        <el-row>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10">
            <el-form-item label="关键字">
              <el-input v-model="state.tableQueryParams.keyword" clearable placeholder="请输入模糊查询关键字"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="盘点单号">
              <el-input v-model="state.tableQueryParams.countNo" clearable placeholder="请输入盘点单号"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="仓库">
              <el-select v-model="state.tableQueryParams.warehouseName" clearable placeholder="请选择仓库">
                <el-option v-for="item in state.dropdownData.warehouseList" :key="item.id" :label="item.warehouseName" :value="item.warehouseName" />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="盘点日期">
              <el-date-picker type="daterange" v-model="state.tableQueryParams.countDateRange"  value-format="YYYY-MM-DD HH:mm:ss" start-placeholder="开始日期" end-placeholder="结束日期" :default-time="[new Date('1 00:00:00'), new Date('1 23:59:59')]" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="状态">
              <el-select v-model="state.tableQueryParams.status" clearable placeholder="请选择状态">
                <el-option v-for="option in statusOptions" :key="option.value" :label="option.label" :value="option.value" />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="差异数量">
              <el-input-number v-model="state.tableQueryParams.diffQuantity"  clearable placeholder="请输入差异数量"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="差异金额">
              <el-input-number v-model="state.tableQueryParams.diffAmount"  clearable placeholder="请输入差异金额"/>
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
                <el-button type="primary"  icon="ele-Search" @click="handleQuery" v-auth="'stoCount:page'" v-reclick="1000"> 查询 </el-button>
                <el-button icon="ele-Refresh" @click="() => state.tableQueryParams = {}"> 重置 </el-button>
                <el-button icon="ele-ZoomIn" @click="() => state.showAdvanceQueryUI = true" v-if="!state.showAdvanceQueryUI" style="margin-left:5px;"> 高级查询 </el-button>
                <el-button icon="ele-ZoomOut" @click="() => state.showAdvanceQueryUI = false" v-if="state.showAdvanceQueryUI" style="margin-left:5px;"> 隐藏 </el-button>
                <el-button type="danger" style="margin-left:5px;" icon="ele-Delete" @click="batchDelStoCount" :disabled="state.selectData.length == 0" v-auth="'stoCount:batchDelete'"> 删除 </el-button>
                <el-button type="primary" style="margin-left:5px;" icon="ele-Plus" @click="editDialogRef.openDialog(null, '新增盘点单')" v-auth="'stoCount:add'"> 新增 </el-button>
                <el-dropdown :show-timeout="70" :hide-timeout="50" @command="exportStoCountCommand">
                  <el-button type="primary" style="margin-left:5px;" icon="ele-FolderOpened" v-reclick="20000" v-auth="'stoCount:export'"> 导出 </el-button>
                  <template #dropdown>
                    <el-dropdown-menu>
                      <el-dropdown-item command="select" :disabled="state.selectData.length == 0">导出选中</el-dropdown-item>
                      <el-dropdown-item command="current">导出本页</el-dropdown-item>
                      <el-dropdown-item command="all">导出全部</el-dropdown-item>
                    </el-dropdown-menu>
                  </template>
                </el-dropdown>
                <el-button type="warning" style="margin-left:5px;" icon="ele-MostlyCloudy" @click="importDataRef.openDialog()" v-auth="'stoCount:import'"> 导入 </el-button>
              </el-button-group>
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
    </el-card>
    <el-card class="full-table" shadow="hover" style="margin-top: 5px">
      <el-table :data="state.tableData" @selection-change="(val: any[]) => { state.selectData = val; }" style="width: 100%" v-loading="state.tableLoading" tooltip-effect="light" row-key="id" @sort-change="sortChange" border>
        <el-table-column type="selection" width="40" align="center" v-if="auth('stoCount:batchDelete') || auth('stoCount:export')" />
        <el-table-column type="index" label="序号" width="55" align="center"/>
        <el-table-column prop='countNo' label='盘点单号' show-overflow-tooltip />
        <el-table-column prop='warehouseCode' label='仓库编码' show-overflow-tooltip />
        <el-table-column prop='warehouseName' label='仓库名称' show-overflow-tooltip />
        <el-table-column prop='countDate' label='盘点日期' show-overflow-tooltip />
        <el-table-column prop='status' label='状态' width="100" align="center">
          <template #default="scope">
            <el-tag :type="getStatusType(scope.row.status)" size="small">
              {{ getStatusText(scope.row.status) }}
            </el-tag>
          </template>
        </el-table-column>
        <el-table-column prop='diffQuantity' label='差异数量' show-overflow-tooltip />
        <el-table-column prop='diffAmount' label='差异金额' show-overflow-tooltip />
        <el-table-column prop='remark' label='备注' show-overflow-tooltip />
        <el-table-column label="修改记录" width="100" align="center" show-overflow-tooltip>
          <template #default="scope">
            <ModifyRecord :data="scope.row" />
          </template>
        </el-table-column>
        <el-table-column label="操作" width="220" align="center" fixed="right" show-overflow-tooltip>
          <template #default="scope">
            <el-button v-if="scope.row.status === 'Draft'" type="success" size="small" text icon="ele-Check" @click="approveStoCount(scope.row)"> 审核 </el-button>
            <el-button v-if="scope.row.status === 'Draft'" type="danger" size="small" text icon="ele-X" @click="rejectStoCount(scope.row)"> 拒绝 </el-button>
            <el-button icon="ele-Edit" size="small" text type="primary" @click="editDialogRef.openDialog(scope.row, '编辑盘点单')" :disabled="scope.row.status !== 'Draft'"> 编辑 </el-button>
            <el-button icon="ele-Delete" size="small" text type="danger" @click="delStoCount(scope.row)" :disabled="scope.row.status !== 'Draft'"> 删除 </el-button>
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
      <ImportData ref="importDataRef" :import="stoCountApi.importData" :download="stoCountApi.downloadTemplate" v-auth="'stoCount:import'" @refresh="handleQuery"/>
      <printDialog ref="printDialogRef" :title="'打印盘点单'" @reloadTable="handleQuery" />
      <editDialog ref="editDialogRef" @reloadTable="handleQuery" />
      
      <!-- 拒绝盘点单对话框 -->
      <el-dialog v-model="rejectDialogVisible" title="拒绝盘点单" width="400px" draggable :close-on-click-modal="false">
        <el-form :model="rejectForm" label-width="100">
          <el-form-item label="盘点单号">
            <el-input v-model="rejectForm.countNo" disabled />
          </el-form-item>
          <el-form-item label="拒绝原因" required>
            <el-input v-model="rejectForm.reason" type="textarea" :rows="3" placeholder="请输入拒绝原因" maxlength="200" show-word-limit />
          </el-form-item>
        </el-form>
        <template #footer>
          <el-button @click="rejectDialogVisible = false">取 消</el-button>
          <el-button type="primary" @click="submitReject">确 定</el-button>
        </template>
      </el-dialog>
    </el-card>
  </div>
</template>
<style scoped>
:deep(.el-input), :deep(.el-select), :deep(.el-input-number) {
  width: 100%;
}
</style>