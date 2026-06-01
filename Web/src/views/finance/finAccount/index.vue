<script lang="ts" setup name="finAccount">
import { ref, reactive, onMounted } from "vue";
import { auth } from '/@/utils/authFunction';
import { ElMessageBox, ElMessage } from "element-plus";
import { downloadStreamFile } from "/@/utils/download";
import { useFinAccountApi } from '/@/api/finance/finAccount';
import editDialog from '/@/views/finance/finAccount/component/editDialog.vue'
import printDialog from '/@/views/system/print/component/hiprint/preview.vue'
import ModifyRecord from '/@/components/table/modifyRecord.vue';
import ImportData from "/@/components/table/importData.vue";

const finAccountApi = useFinAccountApi();
const printDialogRef = ref();
const editDialogRef = ref();
const importDataRef = ref();

// 科目类型映射
const accountTypeMap: Record<string, string> = {
	Asset: '资产',
	Liability: '负债',
	Equity: '权益',
	Cost: '成本',
	Profit: '损益'
};

// 余额方向映射
const directionMap: Record<string, string> = {
	Debit: '借方',
	Credit: '贷方'
};

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
	accountTypes: [] as any[], // 科目类型下拉数据
	directions: [] as any[], // 余额方向下拉数据
	accountSelector: [] as any[], // 科目下拉数据
});

// 页面加载时
onMounted(async () => {
	await loadDropdownData();
});

// 加载下拉数据
const loadDropdownData = async () => {
	try {
		const [typesRes, directionsRes, selectorRes] = await Promise.all([
			finAccountApi.getAccountTypes(),
			finAccountApi.getDirections(),
			finAccountApi.selector()
		]);
		state.accountTypes = typesRes.data.result || [];
		state.directions = directionsRes.data.result || [];
		state.accountSelector = selectorRes.data.result || [];
	} catch (error) {
		console.error('加载下拉数据失败:', error);
	}
};

// 获取科目类型显示文本
const getAccountTypeText = (type: string) => {
	return accountTypeMap[type] || type;
};

// 获取余额方向显示文本
const getDirectionText = (direction: string) => {
	return directionMap[direction] || direction;
};

// 查询操作
const handleQuery = async (params: any = {}) => {
	state.tableLoading = true;
	state.tableParams = Object.assign(state.tableParams, params);
	const result = await finAccountApi.page(Object.assign(state.tableQueryParams, state.tableParams)).then(res => res.data.result);
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
const delFinAccount = (row: any) => {
	ElMessageBox.confirm(`确定要删除吗?`, "提示", {
		confirmButtonText: "确定",
		cancelButtonText: "取消",
		type: "warning",
	}).then(async () => {
		await finAccountApi.delete({ id: row.id });
		handleQuery();
		ElMessage.success("删除成功");
	}).catch(() => {});
};

// 批量删除
const batchDelFinAccount = () => {
	ElMessageBox.confirm(`确定要删除${state.selectData.length}条记录吗?`, "提示", {
		confirmButtonText: "确定",
		cancelButtonText: "取消",
		type: "warning",
	}).then(async () => {
		await finAccountApi.batchDelete(state.selectData.map(u => ({ id: u.id }) )).then(res => {
			ElMessage.success(`成功批量删除${res.data.result}条记录`);
			handleQuery();
		});
	}).catch(() => {});
};

// 导出数据
const exportFinAccountCommand = async (command: string) => {
	try {
		state.exportLoading = true;
		if (command === 'select') {
			const params = Object.assign({}, state.tableQueryParams, state.tableParams, { selectKeyList: state.selectData.map(u => u.id) });
			await finAccountApi.exportData(params).then(res => downloadStreamFile(res));
		} else if (command === 'current') {
			const params = Object.assign({}, state.tableQueryParams, state.tableParams);
			await finAccountApi.exportData(params).then(res => downloadStreamFile(res));
		} else if (command === 'all') {
			const params = Object.assign({}, state.tableQueryParams, state.tableParams, { page: 1, pageSize: 99999999 });
			await finAccountApi.exportData(params).then(res => downloadStreamFile(res));
		}
	} finally {
		state.exportLoading = false;
	}
}

handleQuery();
</script>
<template>
	<div class="finAccount-container" v-loading="state.exportLoading">
		<el-card shadow="hover" :body-style="{ paddingBottom: '0' }"> 
			<el-form :model="state.tableQueryParams" ref="queryForm" labelWidth="90">
				<el-row>
					<el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10">
						<el-form-item label="关键字">
							<el-input v-model="state.tableQueryParams.keyword" clearable placeholder="请输入模糊查询关键字"/>
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
						<el-form-item label="科目全称">
							<el-input v-model="state.tableQueryParams.fullName" clearable placeholder="请输入科目全称"/>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
						<el-form-item label="上级科目">
							<el-select v-model="state.tableQueryParams.parentId" clearable filterable placeholder="请选择上级科目">
								<el-option v-for="item in state.accountSelector" :key="item.id" :label="`${item.accountCode} - ${item.accountName}`" :value="item.id" />
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
						<el-form-item label="科目级次">
							<el-input-number v-model="state.tableQueryParams.level"  clearable placeholder="请输入科目级次"/>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
						<el-form-item label="科目类型">
							<el-select v-model="state.tableQueryParams.accountType" clearable filterable placeholder="请选择科目类型">
								<el-option v-for="item in state.accountTypes" :key="item.code" :label="item.name" :value="item.code" />
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
						<el-form-item label="余额方向">
							<el-select v-model="state.tableQueryParams.direction" clearable filterable placeholder="请选择余额方向">
								<el-option v-for="item in state.directions" :key="item.code" :label="item.name" :value="item.code" />
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
						<el-form-item label="是否明细科目">
								<el-select clearable filterable v-model="state.tableQueryParams.isDetail" placeholder="请选择是否明细科目"> 
									<el-option     value="true" label="是" /> 
									<el-option     value="false" label="否" />  
								</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
						<el-form-item label="是否辅助核算">
								<el-select clearable filterable v-model="state.tableQueryParams.isAuxiliary" placeholder="请选择是否辅助核算"> 
									<el-option     value="true" label="是" /> 
									<el-option     value="false" label="否" />  
								</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
						<el-form-item label="是否现金流量科目">
								<el-select clearable filterable v-model="state.tableQueryParams.isCashFlow" placeholder="请选择是否现金流量科目"> 
									<el-option     value="true" label="是" /> 
									<el-option     value="false" label="否" />  
								</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
						<el-form-item label="部门辅助核算">
								<el-select clearable filterable v-model="state.tableQueryParams.auxDept" placeholder="请选择部门辅助核算"> 
									<el-option     value="true" label="是" /> 
									<el-option     value="false" label="否" />  
								</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
						<el-form-item label="个人辅助核算">
								<el-select clearable filterable v-model="state.tableQueryParams.auxPerson" placeholder="请选择个人辅助核算"> 
									<el-option     value="true" label="是" /> 
									<el-option     value="false" label="否" />  
								</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
						<el-form-item label="项目辅助核算">
								<el-select clearable filterable v-model="state.tableQueryParams.auxProject" placeholder="请选择项目辅助核算"> 
									<el-option     value="true" label="是" /> 
									<el-option     value="false" label="否" />  
								</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
						<el-form-item label="供应商辅助核算">
								<el-select clearable filterable v-model="state.tableQueryParams.auxSupplier" placeholder="请选择供应商辅助核算"> 
									<el-option     value="true" label="是" /> 
									<el-option     value="false" label="否" />  
								</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
						<el-form-item label="客户辅助核算">
								<el-select clearable filterable v-model="state.tableQueryParams.auxCustomer" placeholder="请选择客户辅助核算"> 
									<el-option     value="true" label="是" /> 
									<el-option     value="false" label="否" />  
								</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
						<el-form-item label="存货辅助核算">
								<el-select clearable filterable v-model="state.tableQueryParams.auxInventory" placeholder="请选择存货辅助核算"> 
									<el-option     value="true" label="是" /> 
									<el-option     value="false" label="否" />  
								</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
						<el-form-item label="是否启用">
								<el-select clearable filterable v-model="state.tableQueryParams.isEnabled" placeholder="请选择是否启用"> 
									<el-option     value="true" label="是" /> 
									<el-option     value="false" label="否" />  
								</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
						<el-form-item label="排序号">
							<el-input-number v-model="state.tableQueryParams.sortOrder"  clearable placeholder="请输入排序号"/>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10">
						<el-form-item >
							<el-button-group style="display: flex; align-items: center;">
								<el-button type="primary"  icon="ele-Search" @click="handleQuery" v-auth="'finAccount:page'" v-reclick="1000"> 查询 </el-button>
								<el-button icon="ele-Refresh" @click="() => state.tableQueryParams = {}"> 重置 </el-button>
								<el-button icon="ele-ZoomIn" @click="() => state.showAdvanceQueryUI = true" v-if="!state.showAdvanceQueryUI" style="margin-left:5px;"> 高级查询 </el-button>
								<el-button icon="ele-ZoomOut" @click="() => state.showAdvanceQueryUI = false" v-if="state.showAdvanceQueryUI" style="margin-left:5px;"> 隐藏 </el-button>
								<el-button type="danger" style="margin-left:5px;" icon="ele-Delete" @click="batchDelFinAccount" :disabled="state.selectData.length == 0" v-auth="'finAccount:batchDelete'"> 删除 </el-button>
								<el-button type="primary" style="margin-left:5px;" icon="ele-Plus" @click="editDialogRef.openDialog(null, '新增会计科目表')" v-auth="'finAccount:add'"> 新增 </el-button>
								<el-dropdown :show-timeout="70" :hide-timeout="50" @command="exportFinAccountCommand">
									<el-button type="primary" style="margin-left:5px;" icon="ele-FolderOpened" v-reclick="20000" v-auth="'finAccount:export'"> 导出 </el-button>
									<template #dropdown>
										<el-dropdown-menu>
											<el-dropdown-item command="select" :disabled="state.selectData.length == 0">导出选中</el-dropdown-item>
											<el-dropdown-item command="current">导出本页</el-dropdown-item>
											<el-dropdown-item command="all">导出全部</el-dropdown-item>
										</el-dropdown-menu>
									</template>
								</el-dropdown>
								<el-button type="warning" style="margin-left:5px;" icon="ele-MostlyCloudy" @click="importDataRef.openDialog()" v-auth="'finAccount:import'"> 导入 </el-button>
							</el-button-group>
						</el-form-item>
					</el-col>
				</el-row>
			</el-form>
		</el-card>
		<el-card class="full-table" shadow="hover" style="margin-top: 5px">
			<el-table :data="state.tableData" @selection-change="(val: any[]) => { state.selectData = val; }" style="width: 100%" v-loading="state.tableLoading" tooltip-effect="light" row-key="id" @sort-change="sortChange" border>
				<el-table-column type="selection" width="40" align="center" v-if="auth('finAccount:batchDelete') || auth('finAccount:export')" />
				<el-table-column type="index" label="序号" width="55" align="center"/>
				<el-table-column prop='accountCode' label='科目编码' show-overflow-tooltip />
				<el-table-column prop='accountName' label='科目名称' show-overflow-tooltip />
				<el-table-column prop='fullName' label='科目全称' show-overflow-tooltip />
				<el-table-column prop='parentAccountName' label='上级科目' show-overflow-tooltip />
				<el-table-column prop='level' label='科目级次' show-overflow-tooltip />
				<el-table-column label='科目类型' show-overflow-tooltip>
					<template #default="scope">
						<el-tag type="info">
							{{ getAccountTypeText(scope.row.accountType) }}
						</el-tag>
					</template>
				</el-table-column>
				<el-table-column label='余额方向' show-overflow-tooltip>
					<template #default="scope">
						<el-tag type="success" v-if="scope.row.direction === 'Debit'">借方</el-tag>
						<el-tag type="warning" v-else-if="scope.row.direction === 'Credit'">贷方</el-tag>
						<el-tag v-else>{{ scope.row.direction }}</el-tag>
					</template>
				</el-table-column>
				<el-table-column prop='isDetail' label='是否明细科目' show-overflow-tooltip>
					<template #default="scope">
						<el-tag v-if="scope.row.isDetail"> 是 </el-tag>
						<el-tag type="danger" v-else> 否 </el-tag>
					</template>
				</el-table-column>
				<el-table-column prop='isAuxiliary' label='是否辅助核算' show-overflow-tooltip>
					<template #default="scope">
						<el-tag v-if="scope.row.isAuxiliary"> 是 </el-tag>
						<el-tag type="danger" v-else> 否 </el-tag>
					</template>
				</el-table-column>
				<el-table-column prop='isCashFlow' label='是否现金流量科目' show-overflow-tooltip>
					<template #default="scope">
						<el-tag v-if="scope.row.isCashFlow"> 是 </el-tag>
						<el-tag type="danger" v-else> 否 </el-tag>
					</template>
				</el-table-column>
				<el-table-column prop='auxDept' label='部门辅助核算' show-overflow-tooltip>
					<template #default="scope">
						<el-tag v-if="scope.row.auxDept"> 是 </el-tag>
						<el-tag type="danger" v-else> 否 </el-tag>
					</template>
				</el-table-column>
				<el-table-column prop='auxPerson' label='个人辅助核算' show-overflow-tooltip>
					<template #default="scope">
						<el-tag v-if="scope.row.auxPerson"> 是 </el-tag>
						<el-tag type="danger" v-else> 否 </el-tag>
					</template>
				</el-table-column>
				<el-table-column prop='auxProject' label='项目辅助核算' show-overflow-tooltip>
					<template #default="scope">
						<el-tag v-if="scope.row.auxProject"> 是 </el-tag>
						<el-tag type="danger" v-else> 否 </el-tag>
					</template>
				</el-table-column>
				<el-table-column prop='auxSupplier' label='供应商辅助核算' show-overflow-tooltip>
					<template #default="scope">
						<el-tag v-if="scope.row.auxSupplier"> 是 </el-tag>
						<el-tag type="danger" v-else> 否 </el-tag>
					</template>
				</el-table-column>
				<el-table-column prop='auxCustomer' label='客户辅助核算' show-overflow-tooltip>
					<template #default="scope">
						<el-tag v-if="scope.row.auxCustomer"> 是 </el-tag>
						<el-tag type="danger" v-else> 否 </el-tag>
					</template>
				</el-table-column>
				<el-table-column prop='auxInventory' label='存货辅助核算' show-overflow-tooltip>
					<template #default="scope">
						<el-tag v-if="scope.row.auxInventory"> 是 </el-tag>
						<el-tag type="danger" v-else> 否 </el-tag>
					</template>
				</el-table-column>
				<el-table-column prop='isEnabled' label='是否启用' show-overflow-tooltip>
					<template #default="scope">
						<el-tag v-if="scope.row.isEnabled"> 是 </el-tag>
						<el-tag type="danger" v-else> 否 </el-tag>
					</template>
				</el-table-column>
				<el-table-column prop='sortOrder' label='排序号' show-overflow-tooltip />
				<el-table-column label="修改记录" width="100" align="center" show-overflow-tooltip>
					<template #default="scope">
						<ModifyRecord :data="scope.row" />
					</template>
				</el-table-column>
				<el-table-column label="操作" width="140" align="center" fixed="right" show-overflow-tooltip v-if="auth('finAccount:update') || auth('finAccount:delete')">
					<template #default="scope">
						<el-button icon="ele-Edit" size="small" text type="primary" @click="editDialogRef.openDialog(scope.row, '编辑会计科目表')" v-auth="'finAccount:update'"> 编辑 </el-button>
						<el-button icon="ele-Delete" size="small" text type="primary" @click="delFinAccount(scope.row)" v-auth="'finAccount:delete'"> 删除 </el-button>
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
			<ImportData ref="importDataRef" :import="finAccountApi.importData" :download="finAccountApi.downloadTemplate" v-auth="'finAccount:import'" @refresh="handleQuery"/>
			<printDialog ref="printDialogRef" :title="'打印会计科目表'" @reloadTable="handleQuery" />
			<editDialog ref="editDialogRef" @reloadTable="handleQuery" />
		</el-card>
	</div>
</template>
<style scoped>
:deep(.el-input), :deep(.el-select), :deep(.el-input-number) {
	width: 100%;
}
</style>
