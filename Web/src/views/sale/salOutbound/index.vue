<script lang="ts" setup name="salOutbound">
import { ref, reactive, onMounted } from "vue";
import { auth } from '/@/utils/authFunction';
import { ElMessageBox, ElMessage } from "element-plus";
import { formatDate } from "/@/utils/formatTime";
import { downloadStreamFile } from "/@/utils/download";
import { useSalOutboundApi } from '/@/api/sale/salOutbound';
import editDialog from '/@/views/sale/salOutbound/component/editDialog.vue'
import printDialog from '/@/views/system/print/component/hiprint/preview.vue'
import ModifyRecord from '/@/components/table/modifyRecord.vue';
import ImportData from "/@/components/table/importData.vue";

const salOutboundApi = useSalOutboundApi();
const printDialogRef = ref();
const editDialogRef = ref();
const importDataRef = ref();
const state = reactive({
	exportLoading: false,
	tableLoading: false,
	showAdvanceQueryUI: false,
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
	statusType: {
		Draft: { text: '草稿', type: 'info' },
		Approved: { text: '已审核', type: 'success' },
		Rejected: { text: '已拒绝', type: 'danger' },
		PartialDelivery: { text: '部分发货', type: 'warning' },
		Confirmed: { text: '已完成', type: 'primary' },
		Cancelled: { text: '已取消', type: 'info' },
	},
	shippingMethodType: {
		Express: { text: '快递', type: 'primary' },
		Logistics: { text: '物流', type: 'success' },
		Delivery: { text: '送货上门', type: 'warning' },
		Pickup: { text: '自提', type: 'info' },
	},
});

onMounted(async () => {
});

const handleQuery = async (params: any = {}) => {
	state.tableLoading = true;
	state.tableParams = Object.assign(state.tableParams, params);
	const result = await salOutboundApi.page(Object.assign(state.tableQueryParams, state.tableParams)).then(res => res.data.result);
	state.tableParams.total = result?.total;
	state.tableData = result?.items || [];
	state.tableLoading = false;
};

const sortChange = async (column: any) => {
	state.tableParams.field = column.prop;
	state.tableParams.order = column.order;
	await handleQuery();
};

const delSalOutbound = (row: any) => {
	ElMessageBox.confirm(`确定要删除销售出库单【${row.outboundNo}】吗?`, "提示", {
		confirmButtonText: "确定",
		cancelButtonText: "取消",
		type: "warning",
	}).then(async () => {
		await salOutboundApi.delete({ id: row.id });
		handleQuery();
		ElMessage.success("删除成功");
	}).catch(() => {});
};

const batchDelSalOutbound = () => {
	ElMessageBox.confirm(`确定要删除${state.selectData.length}条记录吗?`, "提示", {
		confirmButtonText: "确定",
		cancelButtonText: "取消",
		type: "warning",
	}).then(async () => {
		await salOutboundApi.batchDelete(state.selectData.map(u => ({ id: u.id })));
		ElMessage.success(`成功批量删除${res.data.result}条记录`);
		handleQuery();
	}).catch(() => {});
};

// 审核通过（简化流程：草稿→已审核，自动生成库存出库单）
const approveSalOutbound = (row: any) => {
	ElMessageBox.confirm(`确定要审核销售出库单【${row.outboundNo}】吗？\n审核通过后将自动生成库存出库单！`, "审核通过", {
		confirmButtonText: "确定",
		cancelButtonText: "取消",
		type: "success",
	}).then(async () => {
		const res = await salOutboundApi.approve({ 
			id: row.id,
			approvalRemark: ''
		});
		ElMessage.success(res.data.result?.message || res.data || '审核通过！');
		handleQuery();
	}).catch(() => {});
};

// 拒绝销售出库单
const rejectSalOutbound = (row: any) => {
	ElMessageBox.prompt('请输入拒绝原因', '审核拒绝', {
		confirmButtonText: '确定',
		cancelButtonText: '取消',
		inputErrorMessage: '拒绝原因不能为空',
		inputValidator: (value: string) => {
			if (!value || !value.trim()) {
				return '请输入拒绝原因';
			}
			return true;
		}
	}).then(async ({ value }) => {
		const res = await salOutboundApi.reject({ 
			id: row.id,
			rejectReason: value
		});
		ElMessage.success(res.data.result || '已拒绝！');
		handleQuery();
	}).catch(() => {});
};

// 判断是否可以审核（草稿或已拒绝状态）
const canApprove = (status: string) => {
	return status === 'Draft' || status === 'Rejected';
};

// 判断是否可以拒绝（草稿或已审核状态）
const canReject = (status: string) => {
	return status === 'Draft' || status === 'Approved';
};

const exportSalOutboundCommand = async (command: string) => {
	try {
		state.exportLoading = true;
		if (command === 'select') {
			const params = Object.assign({}, state.tableQueryParams, state.tableParams, { selectKeyList: state.selectData.map(u => u.id) });
			await salOutboundApi.exportData(params).then(res => downloadStreamFile(res));
		} else if (command === 'current') {
			const params = Object.assign({}, state.tableQueryParams, state.tableParams);
			await salOutboundApi.exportData(params).then(res => downloadStreamFile(res));
		} else if (command === 'all') {
			const params = Object.assign({}, state.tableQueryParams, state.tableParams, { page: 1, pageSize: 99999999 });
			await salOutboundApi.exportData(params).then(res => downloadStreamFile(res));
		}
	} finally {
		state.exportLoading = false;
	}
};

// 获取配送方式显示文本
const getShippingMethodText = (method: string) => {
	return state.shippingMethodType[method]?.text || method;
};

handleQuery();
</script>
<template>
	<div class="salOutbound-container" v-loading="state.exportLoading">
		<el-card shadow="hover" :body-style="{ paddingBottom: '0' }"> 
			<el-form :model="state.tableQueryParams" ref="queryForm" label-width="90">
				<el-row>
					<el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10">
						<el-form-item label="关键字">
							<el-input v-model="state.tableQueryParams.keyword" clearable placeholder="请输入模糊查询关键字"/>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
						<el-form-item label="出库单号">
							<el-input v-model="state.tableQueryParams.outboundNo" clearable placeholder="请输入出库单号"/>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
						<el-form-item label="订单号">
							<el-input v-model="state.tableQueryParams.orderNo" clearable placeholder="请输入订单号"/>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
						<el-form-item label="客户名称">
							<el-input v-model="state.tableQueryParams.customerName" clearable placeholder="请输入客户名称"/>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
						<el-form-item label="仓库">
							<el-input v-model="state.tableQueryParams.warehouseName" clearable placeholder="请输入仓库名称"/>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
						<el-form-item label="配送方式">
							<el-select v-model="state.tableQueryParams.shippingMethod" placeholder="请选择配送方式" clearable style="width: 100%;">
								<el-option label="快递" value="Express" />
								<el-option label="物流" value="Logistics" />
								<el-option label="送货上门" value="Delivery" />
								<el-option label="自提" value="Pickup" />
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="state.showAdvanceQueryUI">
						<el-form-item label="状态">
							<el-select v-model="state.tableQueryParams.status" placeholder="请选择状态" clearable style="width: 100%;">
								<el-option label="草稿" value="Draft" />
								<el-option label="已确认" value="Confirmed" />
								<el-option label="已取消" value="Cancelled" />
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10">
						<el-form-item >
							<el-button-group style="display: flex; align-items: center;">
								<el-button type="primary" icon="ele-Search" @click="handleQuery" v-auth="'salOutbound:page'" v-reclick="1000"> 查询 </el-button>
								<el-button icon="ele-Refresh" @click="() => state.tableQueryParams = {}"> 重置 </el-button>
								<el-button icon="ele-ZoomIn" @click="() => state.showAdvanceQueryUI = true" v-if="!state.showAdvanceQueryUI" style="margin-left:5px;"> 高级查询 </el-button>
								<el-button icon="ele-ZoomOut" @click="() => state.showAdvanceQueryUI = false" v-if="state.showAdvanceQueryUI" style="margin-left:5px;"> 隐藏 </el-button>
							</el-button-group>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb10">
						<el-form-item >
							<el-button-group style="display: flex; align-items: center;">
								<el-button type="success" icon="ele-DocumentCopy" @click="editDialogRef.openDialog('create')" v-auth="'salOutbound:add'" v-reclick="1000"> 从订单创建 </el-button>
								<el-button type="primary" icon="ele-Plus" @click="editDialogRef.openDialog('manual')" v-auth="'salOutbound:add'" style="margin-left:5px;"> 手工创建 </el-button>
								<el-button type="danger" icon="ele-Delete" @click="batchDelSalOutbound" :disabled="state.selectData.length == 0" v-auth="'salOutbound:batchDelete'" style="margin-left:5px;"> 删除 </el-button>
								<el-dropdown :show-timeout="70" :hide-timeout="50" @command="exportSalOutboundCommand" style="margin-left:5px;">
									<el-button type="primary" icon="ele-FolderOpened" v-reclick="20000" v-auth="'salOutbound:export'"> 导出 </el-button>
									<template #dropdown>
										<el-dropdown-menu>
											<el-dropdown-item command="select" :disabled="state.selectData.length == 0">导出选中</el-dropdown-item>
											<el-dropdown-item command="current">导出本页</el-dropdown-item>
											<el-dropdown-item command="all">导出全部</el-dropdown-item>
										</el-dropdown-menu>
									</template>
								</el-dropdown>
								<el-button type="warning" icon="ele-Upload" @click="importDataRef.openDialog()" v-auth="'salOutbound:import'" style="margin-left:5px;"> 导入 </el-button>
							</el-button-group>
						</el-form-item>
					</el-col>
				</el-row>
			</el-form>
		</el-card>
		<el-card class="full-table" shadow="hover" style="margin-top: 5px">
			<el-table :data="state.tableData" @selection-change="(val: any[]) => { state.selectData = val; }" style="width: 100%" v-loading="state.tableLoading" tooltip-effect="light" row-key="id" @sort-change="sortChange" border>
				<el-table-column type="selection" width="40" align="center" v-if="auth('salOutbound:batchDelete') || auth('salOutbound:export')" />
				<el-table-column type="index" label="序号" width="55" align="center"/>
				<el-table-column prop="outboundNo" label="出库单号" width="150" show-overflow-tooltip />
				<el-table-column prop="orderNo" label="订单号" width="150" show-overflow-tooltip />
				<el-table-column prop="customerName" label="客户名称" width="150" show-overflow-tooltip />
				<el-table-column prop="warehouseName" label="仓库" width="120" show-overflow-tooltip />
				<el-table-column prop="outboundDate" label="出库日期" width="120" show-overflow-tooltip>
					<template #default="scope">
						{{ scope.row.outboundDate ? formatDate(scope.row.outboundDate) : '-' }}
					</template>
				</el-table-column>
				<el-table-column prop="shippingMethod" label="配送方式" width="100" show-overflow-tooltip>
					<template #default="scope">
						{{ getShippingMethodText(scope.row.shippingMethod) }}
					</template>
				</el-table-column>
				<el-table-column prop="totalQuantity" label="数量" width="80" align="right" show-overflow-tooltip />
				<el-table-column prop="totalAmount" label="金额" width="100" align="right" show-overflow-tooltip>
					<template #default="scope">
						{{ scope.row.totalAmount?.toFixed(2) }}
					</template>
				</el-table-column>
				<el-table-column prop="status" label="状态" width="100" align="center" show-overflow-tooltip>
					<template #default="scope">
						<el-tag :type="state.statusType[scope.row.status]?.type || 'info'" size="small">
							{{ state.statusType[scope.row.status]?.text || scope.row.status }}
						</el-tag>
					</template>
				</el-table-column>
				<el-table-column prop="remark" label="备注" show-overflow-tooltip />
				<el-table-column label="修改记录" width="80" align="center" show-overflow-tooltip>
					<template #default="scope">
						<ModifyRecord :data="scope.row" />
					</template>
				</el-table-column>
				<el-table-column label="操作" width="200" align="center" fixed="right" show-overflow-tooltip v-if="auth('salOutbound:update') || auth('salOutbound:delete') || auth('salOutbound:approve')">
					<template #default="scope">
						<el-button v-if="canApprove(scope.row.status)" type="success" size="small" text icon="ele-Check" @click="approveSalOutbound(scope.row)" v-auth="'salOutbound:approve'"> 审核 </el-button>
						<el-button v-if="canReject(scope.row.status)" type="danger" size="small" text icon="ele-X" @click="rejectSalOutbound(scope.row)" v-auth="'salOutbound:reject'"> 拒绝 </el-button>
						<el-button icon="ele-Edit" size="small" text type="primary" @click="editDialogRef.openDialog(scope.row)" v-auth="'salOutbound:update'" :disabled="scope.row.status !== 'Draft' && scope.row.status !== 'Rejected'"> 编辑 </el-button>
						<el-button icon="ele-Delete" size="small" text type="danger" @click="delSalOutbound(scope.row)" v-auth="'salOutbound:delete'" :disabled="scope.row.status === 'Approved' || scope.row.status === 'PartialDelivery' || scope.row.status === 'Confirmed'"> 删除 </el-button>
					</template>
				</el-table-column>
			</el-table>
			<el-pagination 
				v-model:currentPage="state.tableParams.page"
				v-model:pageSize="state.tableParams.pageSize"
				@size-change="(val: any) => handleQuery({ pageSize: val })"
				@current-change="(val: any) => handleQuery({ page: val })"
				layout="total, sizes, prev, pager, next, jumper"
				:page-sizes="[10, 20, 50, 100, 200, 500]"
				:total="state.tableParams.total"
				size="small"
				background />
			<ImportData ref="importDataRef" :import="salOutboundApi.importData" :download="salOutboundApi.downloadTemplate" v-auth="'salOutbound:import'" @refresh="handleQuery"/>
			<printDialog ref="printDialogRef" :title="'打印销售出库'" @reloadTable="handleQuery" />
			<editDialog ref="editDialogRef" @reloadTable="handleQuery" />
		</el-card>
	</div>
</template>
<style scoped>
:deep(.el-input), :deep(.el-select), :deep(.el-input-number) {
  width: 100%;
}
</style>