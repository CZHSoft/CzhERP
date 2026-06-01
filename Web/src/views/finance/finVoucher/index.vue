<script lang="ts" setup name="finVoucher">
import { ref, reactive, onMounted } from "vue";
import { auth } from '/@/utils/authFunction';
import { ElMessageBox, ElMessage } from "element-plus";
import { downloadStreamFile } from "/@/utils/download";
import { useFinVoucherApi } from '/@/api/finance/finVoucher';
import editDialog from '/@/views/finance/finVoucher/component/editDialog.vue'
import printDialog from '/@/views/system/print/component/hiprint/preview.vue'
import ModifyRecord from '/@/components/table/modifyRecord.vue';
import ImportData from "/@/components/table/importData.vue";

const finVoucherApi = useFinVoucherApi();
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
  const result = await finVoucherApi.page(Object.assign(state.tableQueryParams, state.tableParams)).then(res => res.data.result);
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
const delFinVoucher = (row: any) => {
  ElMessageBox.confirm(`确定要删除吗?`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  }).then(async () => {
    await finVoucherApi.delete({ id: row.id });
    handleQuery();
    ElMessage.success("删除成功");
  }).catch(() => {});
};

// 批量删除
const batchDelFinVoucher = () => {
  ElMessageBox.confirm(`确定要删除${state.selectData.length}条记录吗?`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  }).then(async () => {
    await finVoucherApi.batchDelete(state.selectData.map(u => ({ id: u.id }) )).then(res => {
      ElMessage.success(`成功批量删除${res.data.result}条记录`);
      handleQuery();
    });
  }).catch(() => {});
};

// 导出数据
const exportFinVoucherCommand = async (command: string) => {
  try {
    state.exportLoading = true;
    if (command === 'select') {
      const params = Object.assign({}, state.tableQueryParams, state.tableParams, { selectKeyList: state.selectData.map(u => u.id) });
      await finVoucherApi.exportData(params).then(res => downloadStreamFile(res));
    } else if (command === 'current') {
      const params = Object.assign({}, state.tableQueryParams, state.tableParams);
      await finVoucherApi.exportData(params).then(res => downloadStreamFile(res));
    } else if (command === 'all') {
      const params = Object.assign({}, state.tableQueryParams, state.tableParams, { page: 1, pageSize: 99999999 });
      await finVoucherApi.exportData(params).then(res => downloadStreamFile(res));
    }
  } finally {
    state.exportLoading = false;
  }
}

handleQuery();
</script>
<template>
  <div class="finVoucher-container" v-loading="state.exportLoading">
    <el-card shadow="hover" :body-style="{ paddingBottom: '0' }"> 
      <el-form :model="state.tableQueryParams" ref="queryForm" labelWidth="90">
        <el-row>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10">
            <el-form-item label="关键字">
              <el-input v-model="state.tableQueryParams.keyword" clearable placeholder="请输入模糊查询关键字"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="凭证号">
              <el-input v-model="state.tableQueryParams.voucherNo" clearable placeholder="请输入凭证号"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="凭证字">
              <el-input v-model="state.tableQueryParams.voucherWord" clearable placeholder="请输入凭证字"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="凭证日期">
              <el-date-picker type="daterange" v-model="state.tableQueryParams.voucherDateRange"  value-format="YYYY-MM-DD HH:mm:ss" start-placeholder="开始日期" end-placeholder="结束日期" :default-time="[new Date('1 00:00:00'), new Date('1 23:59:59')]" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="会计年度">
              <el-input-number v-model="state.tableQueryParams.year"  clearable placeholder="请输入会计年度"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="会计期间">
              <el-input-number v-model="state.tableQueryParams.period"  clearable placeholder="请输入会计期间"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="附件数量">
              <el-input-number v-model="state.tableQueryParams.attachmentCount"  clearable placeholder="请输入附件数量"/>
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
            <el-form-item label="借方金额合计">
              <el-input-number v-model="state.tableQueryParams.totalDebit"  clearable placeholder="请输入借方金额合计"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="贷方金额合计">
              <el-input-number v-model="state.tableQueryParams.totalCredit"  clearable placeholder="请输入贷方金额合计"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="状态">
              <el-input v-model="state.tableQueryParams.status" clearable placeholder="请输入状态"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="制单人ID">
              <el-input v-model="state.tableQueryParams.makerId" clearable placeholder="请输入制单人ID"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="制单人姓名">
              <el-input v-model="state.tableQueryParams.makerName" clearable placeholder="请输入制单人姓名"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="制单时间">
              <el-date-picker type="daterange" v-model="state.tableQueryParams.makeTimeRange"  value-format="YYYY-MM-DD HH:mm:ss" start-placeholder="开始日期" end-placeholder="结束日期" :default-time="[new Date('1 00:00:00'), new Date('1 23:59:59')]" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="审核人ID">
              <el-input v-model="state.tableQueryParams.approverId" clearable placeholder="请输入审核人ID"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="审核人姓名">
              <el-input v-model="state.tableQueryParams.approverName" clearable placeholder="请输入审核人姓名"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="审核时间">
              <el-date-picker type="daterange" v-model="state.tableQueryParams.approveTimeRange"  value-format="YYYY-MM-DD HH:mm:ss" start-placeholder="开始日期" end-placeholder="结束日期" :default-time="[new Date('1 00:00:00'), new Date('1 23:59:59')]" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="过账人ID">
              <el-input v-model="state.tableQueryParams.posterId" clearable placeholder="请输入过账人ID"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="过账人姓名">
              <el-input v-model="state.tableQueryParams.posterName" clearable placeholder="请输入过账人姓名"/>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
            <el-form-item label="过账时间">
              <el-date-picker type="daterange" v-model="state.tableQueryParams.postTimeRange"  value-format="YYYY-MM-DD HH:mm:ss" start-placeholder="开始日期" end-placeholder="结束日期" :default-time="[new Date('1 00:00:00'), new Date('1 23:59:59')]" />
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
                <el-button type="primary"  icon="ele-Search" @click="handleQuery" v-auth="'finVoucher:page'" v-reclick="1000"> 查询 </el-button>
                <el-button icon="ele-Refresh" @click="() => state.tableQueryParams = {}"> 重置 </el-button>
                <el-button icon="ele-ZoomIn" @click="() => state.showAdvanceQueryUI = true" v-if="!state.showAdvanceQueryUI" style="margin-left:5px;"> 高级查询 </el-button>
                <el-button icon="ele-ZoomOut" @click="() => state.showAdvanceQueryUI = false" v-if="state.showAdvanceQueryUI" style="margin-left:5px;"> 隐藏 </el-button>
                <el-button type="danger" style="margin-left:5px;" icon="ele-Delete" @click="batchDelFinVoucher" :disabled="state.selectData.length == 0" v-auth="'finVoucher:batchDelete'"> 删除 </el-button>
                <el-button type="primary" style="margin-left:5px;" icon="ele-Plus" @click="editDialogRef.openDialog(null, '新增凭证主表')" v-auth="'finVoucher:add'"> 新增 </el-button>
                <el-dropdown :show-timeout="70" :hide-timeout="50" @command="exportFinVoucherCommand">
                  <el-button type="primary" style="margin-left:5px;" icon="ele-FolderOpened" v-reclick="20000" v-auth="'finVoucher:export'"> 导出 </el-button>
                  <template #dropdown>
                    <el-dropdown-menu>
                      <el-dropdown-item command="select" :disabled="state.selectData.length == 0">导出选中</el-dropdown-item>
                      <el-dropdown-item command="current">导出本页</el-dropdown-item>
                      <el-dropdown-item command="all">导出全部</el-dropdown-item>
                    </el-dropdown-menu>
                  </template>
                </el-dropdown>
                <el-button type="warning" style="margin-left:5px;" icon="ele-MostlyCloudy" @click="importDataRef.openDialog()" v-auth="'finVoucher:import'"> 导入 </el-button>
              </el-button-group>
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
    </el-card>
    <el-card class="full-table" shadow="hover" style="margin-top: 5px">
      <el-table :data="state.tableData" @selection-change="(val: any[]) => { state.selectData = val; }" style="width: 100%" v-loading="state.tableLoading" tooltip-effect="light" row-key="id" @sort-change="sortChange" border>
        <el-table-column type="selection" width="40" align="center" v-if="auth('finVoucher:batchDelete') || auth('finVoucher:export')" />
        <el-table-column type="index" label="序号" width="55" align="center"/>
        <el-table-column prop='voucherNo' label='凭证号' show-overflow-tooltip />
        <el-table-column prop='voucherWord' label='凭证字' show-overflow-tooltip />
        <el-table-column prop='voucherDate' label='凭证日期' show-overflow-tooltip />
        <el-table-column prop='year' label='会计年度' show-overflow-tooltip />
        <el-table-column prop='period' label='会计期间' show-overflow-tooltip />
        <el-table-column prop='attachmentCount' label='附件数量' show-overflow-tooltip />
        <el-table-column prop='sourceType' label='来源单据类型' show-overflow-tooltip />
        <el-table-column prop='sourceId' label='来源单据ID' show-overflow-tooltip />
        <el-table-column prop='sourceNo' label='来源单据号' show-overflow-tooltip />
        <el-table-column prop='totalDebit' label='借方金额合计' show-overflow-tooltip />
        <el-table-column prop='totalCredit' label='贷方金额合计' show-overflow-tooltip />
        <el-table-column prop='status' label='状态' show-overflow-tooltip />
        <el-table-column prop='makerId' label='制单人ID' show-overflow-tooltip />
        <el-table-column prop='makerName' label='制单人姓名' show-overflow-tooltip />
        <el-table-column prop='makeTime' label='制单时间' show-overflow-tooltip />
        <el-table-column prop='approverId' label='审核人ID' show-overflow-tooltip />
        <el-table-column prop='approverName' label='审核人姓名' show-overflow-tooltip />
        <el-table-column prop='approveTime' label='审核时间' show-overflow-tooltip />
        <el-table-column prop='posterId' label='过账人ID' show-overflow-tooltip />
        <el-table-column prop='posterName' label='过账人姓名' show-overflow-tooltip />
        <el-table-column prop='postTime' label='过账时间' show-overflow-tooltip />
        <el-table-column prop='remark' label='备注' show-overflow-tooltip />
        <el-table-column label="修改记录" width="100" align="center" show-overflow-tooltip>
          <template #default="scope">
            <ModifyRecord :data="scope.row" />
          </template>
        </el-table-column>
        <el-table-column label="操作" width="140" align="center" fixed="right" show-overflow-tooltip v-if="auth('finVoucher:update') || auth('finVoucher:delete')">
          <template #default="scope">
            <el-button icon="ele-Edit" size="small" text type="primary" @click="editDialogRef.openDialog(scope.row, '编辑凭证主表')" v-auth="'finVoucher:update'"> 编辑 </el-button>
            <el-button icon="ele-Delete" size="small" text type="primary" @click="delFinVoucher(scope.row)" v-auth="'finVoucher:delete'"> 删除 </el-button>
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
      <ImportData ref="importDataRef" :import="finVoucherApi.importData" :download="finVoucherApi.downloadTemplate" v-auth="'finVoucher:import'" @refresh="handleQuery"/>
      <printDialog ref="printDialogRef" :title="'打印凭证主表'" @reloadTable="handleQuery" />
      <editDialog ref="editDialogRef" @reloadTable="handleQuery" />
    </el-card>
  </div>
</template>
<style scoped>
:deep(.el-input), :deep(.el-select), :deep(.el-input-number) {
  width: 100%;
}
</style>