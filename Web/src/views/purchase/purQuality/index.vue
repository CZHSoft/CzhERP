<script lang="ts" setup name="purQuality">
import { ref, reactive, onMounted } from "vue";
import { auth } from '/@/utils/authFunction';
import { ElMessageBox, ElMessage } from "element-plus";
import { downloadStreamFile } from "/@/utils/download";
import { usePurQualityApi } from '/@/api/purchase/purQuality';
import editDialog from '/@/views/purchase/purQuality/component/editDialog.vue'
import printDialog from '/@/views/system/print/component/hiprint/preview.vue'
import ModifyRecord from '/@/components/table/modifyRecord.vue';
import ImportData from "/@/components/table/importData.vue";

const purQualityApi = usePurQualityApi();
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
});

// 页面加载时
onMounted(async () => {
});

// 查询操作
const handleQuery = async (params: any = {}) => {
  state.tableLoading = true;
  state.tableParams = Object.assign(state.tableParams, params);
  const result = await purQualityApi.page(Object.assign(state.tableQueryParams, state.tableParams)).then(res => res.data.result);
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

// 获取检验类型文本
const getInspectionTypeText = (type: number) => {
  const typeMap: Record<number, string> = {
    1: '全检',
    2: '抽检'
  };
  return typeMap[type] ?? '未知';
};

// 获取检验结果文本
const getResultText = (result: number) => {
  const resultMap: Record<number, string> = {
    0: '待判定',
    1: '合格',
    2: '不合格',
    3: '让步接收'
  };
  return resultMap[result] ?? '未知';
};

// 获取检验结果标签类型
const getResultType = (result: number) => {
  const typeMap: Record<number, string> = {
    0: 'info',
    1: 'success',
    2: 'danger',
    3: 'warning'
  };
  return typeMap[result] ?? 'info';
};

// 删除
const delPurQuality = (row: any) => {
  ElMessageBox.confirm(`确定要删除吗?`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  }).then(async () => {
    await purQualityApi.delete({ id: row.id });
    handleQuery();
    ElMessage.success("删除成功");
  }).catch(() => {});
};

// 审核
const approvePurQuality = (row: any) => {
  ElMessageBox.confirm(`确定要审核该质检记录吗?审核后结果将同步到入库单。`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  }).then(async () => {
    await purQualityApi.approve(row.id);
    handleQuery();
    ElMessage.success("审核成功");
  }).catch(() => {});
};

// 批量删除
const batchDelPurQuality = () => {
  ElMessageBox.confirm(`确定要删除${state.selectData.length}条记录吗?`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  }).then(async () => {
    await purQualityApi.batchDelete(state.selectData.map(u => ({ id: u.id }) )).then(res => {
      ElMessage.success(`成功批量删除${res.data.result}条记录`);
      handleQuery();
    });
  }).catch(() => {});
};

// 导出数据
const exportPurQualityCommand = async (command: string) => {
  try {
    state.exportLoading = true;
    if (command === 'select') {
      const params = Object.assign({}, state.tableQueryParams, state.tableParams, { selectKeyList: state.selectData.map(u => u.id) });
      await purQualityApi.exportData(params).then(res => downloadStreamFile(res));
    } else if (command === 'current') {
      const params = Object.assign({}, state.tableQueryParams, state.tableParams);
      await purQualityApi.exportData(params).then(res => downloadStreamFile(res));
    } else if (command === 'all') {
      const params = Object.assign({}, state.tableQueryParams, state.tableParams, { page: 1, pageSize: 99999999 });
      await purQualityApi.exportData(params).then(res => downloadStreamFile(res));
    }
  } finally {
    state.exportLoading = false;
  }
}

handleQuery();
</script>
<template>
  <div class="purQuality-container" v-loading="state.exportLoading">
    <el-card shadow="hover" :body-style="{ paddingBottom: '0' }"> 
      <el-form :model="state.tableQueryParams" ref="queryForm" labelWidth="90">
        <el-row>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10">
            <el-form-item label="关键字">
              <el-input v-model="state.tableQueryParams.keyword" clearable placeholder="请输入模糊查询关键字"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="质检单号">
              <el-input v-model="state.tableQueryParams.qualityNo" clearable placeholder="请输入质检单号"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="检验类型">
              <el-select v-model="state.tableQueryParams.inspectionType" clearable placeholder="请选择检验类型">
                <el-option label="全检" :value="1" />
                <el-option label="抽检" :value="2" />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="抽样数量">
              <el-input-number v-model="state.tableQueryParams.sampleQty" clearable placeholder="请输入抽样数量"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="检验结果">
              <el-select v-model="state.tableQueryParams.result" clearable placeholder="请选择检验结果">
                <el-option label="待判定" :value="0" />
                <el-option label="合格" :value="1" />
                <el-option label="不合格" :value="2" />
                <el-option label="让步接收" :value="3" />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="检验日期">
              <el-date-picker type="daterange" v-model="state.tableQueryParams.inspectionDateRange" value-format="YYYY-MM-DD HH:mm:ss" start-placeholder="开始日期" end-placeholder="结束日期" :default-time="[new Date('1 00:00:00'), new Date('1 23:59:59')]" />
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
                <el-button type="primary" icon="ele-Search" @click="handleQuery" v-auth="'purQuality:page'" v-reclick="1000"> 查询 </el-button>
                <el-button icon="ele-Refresh" @click="() => state.tableQueryParams = {}"> 重置 </el-button>
                <el-button icon="ele-ZoomIn" @click="() => state.showAdvanceQueryUI = true" v-if="!state.showAdvanceQueryUI" style="margin-left:5px;"> 高级查询 </el-button>
                <el-button icon="ele-ZoomOut" @click="() => state.showAdvanceQueryUI = false" v-if="state.showAdvanceQueryUI" style="margin-left:5px;"> 隐藏 </el-button>
                <el-button type="danger" style="margin-left:5px;" icon="ele-Delete" @click="batchDelPurQuality" :disabled="state.selectData.length == 0" v-auth="'purQuality:batchDelete'"> 删除 </el-button>
                <el-button type="primary" style="margin-left:5px;" icon="ele-Plus" @click="editDialogRef.openDialog(null, '新增质检记录')" v-auth="'purQuality:add'"> 新增 </el-button>
                <el-dropdown :show-timeout="70" :hide-timeout="50" @command="exportPurQualityCommand">
                  <el-button type="primary" style="margin-left:5px;" icon="ele-FolderOpened" v-reclick="20000" v-auth="'purQuality:export'"> 导出 </el-button>
                  <template #dropdown>
                    <el-dropdown-menu>
                      <el-dropdown-item command="select" :disabled="state.selectData.length == 0">导出选中</el-dropdown-item>
                      <el-dropdown-item command="current">导出本页</el-dropdown-item>
                      <el-dropdown-item command="all">导出全部</el-dropdown-item>
                    </el-dropdown-menu>
                  </template>
                </el-dropdown>
                <el-button type="warning" style="margin-left:5px;" icon="ele-MostlyCloudy" @click="importDataRef.openDialog()" v-auth="'purQuality:import'"> 导入 </el-button>
              </el-button-group>
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
    </el-card>
    <el-card class="full-table" shadow="hover" style="margin-top: 5px">
      <el-table :data="state.tableData" @selection-change="(val: any[]) => { state.selectData = val; }" style="width: 100%" v-loading="state.tableLoading" tooltip-effect="light" row-key="id" @sort-change="sortChange" border>
        <el-table-column type="selection" width="40" align="center" v-if="auth('purQuality:batchDelete') || auth('purQuality:export')" />
        <el-table-column type="index" label="序号" width="55" align="center"/>
        <el-table-column prop='qualityNo' label='质检单号' width="140" show-overflow-tooltip />
        <el-table-column prop='inboundNo' label='入库单号' width="140" show-overflow-tooltip />
        <el-table-column label="检验类型" width="100" align="center">
          <template #default="scope">
            {{ getInspectionTypeText(scope.row.inspectionType) }}
          </template>
        </el-table-column>
        <el-table-column prop='sampleQty' label='抽样数量' width="100" show-overflow-tooltip />
        <el-table-column prop='passQty' label='合格数量' width="100" show-overflow-tooltip />
        <el-table-column prop='failQty' label='不合格数量' width="100" show-overflow-tooltip />
        <el-table-column label="检验结果" width="100" align="center">
          <template #default="scope">
            <el-tag :type="getResultType(scope.row.result)" size="small">{{ getResultText(scope.row.result) }}</el-tag>
          </template>
        </el-table-column>
        <el-table-column prop='inspectorId' label='检验员ID' width="100" show-overflow-tooltip />
        <el-table-column prop='inspectionDate' label='检验日期' width="160" show-overflow-tooltip />
        <el-table-column prop='remark' label='备注' show-overflow-tooltip />
        <el-table-column label="修改记录" width="100" align="center" show-overflow-tooltip>
          <template #default="scope">
            <ModifyRecord :data="scope.row" />
          </template>
        </el-table-column>
        <el-table-column label="操作" width="200" align="center" fixed="right" show-overflow-tooltip>
          <template #default="scope">
            <el-button v-if="scope.row.result !== 0" icon="ele-Check" size="small" text type="success" @click="approvePurQuality(scope.row)"> 审核 </el-button>
            <el-button icon="ele-Edit" size="small" text type="primary" @click="editDialogRef.openDialog(scope.row, '编辑质检记录')" v-auth="'purQuality:update'"> 编辑 </el-button>
            <el-button icon="ele-Delete" size="small" text type="primary" @click="delPurQuality(scope.row)" v-auth="'purQuality:delete'"> 删除 </el-button>
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
      <ImportData ref="importDataRef" :import="purQualityApi.importData" :download="purQualityApi.downloadTemplate" v-auth="'purQuality:import'" @refresh="handleQuery"/>
      <printDialog ref="printDialogRef" :title="'打印质检记录'" @reloadTable="handleQuery" />
      <editDialog ref="editDialogRef" @reloadTable="handleQuery" />
    </el-card>
  </div>
</template>
<style scoped>
:deep(.el-input), :deep(.el-select), :deep(.el-input-number) {
  width: 100%;
}
</style>
