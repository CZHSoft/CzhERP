<script lang="ts" name="salOutboundEditDialog" setup>
import { ref, reactive, onMounted } from "vue";
import { ElMessage } from "element-plus";
import type { FormRules } from "element-plus";
import { useSalOutboundApi } from '/@/api/sale/salOutbound';
import { getOrderList } from '/@/api/sale/salOrder';
import { getWarehouseList } from '/@/api/stock/stoWarehouse';

const emit = defineEmits(["reloadTable"]);
const salOutboundApi = useSalOutboundApi();
const ruleFormRef = ref();

const state = reactive({
	title: '创建销售出库单',
	loading: false,
	showDialog: false,
	mode: 'create', // create: 从订单创建, manual: 手工创建
	ruleForm: {} as any,
	orderList: [] as any[],
	warehouseList: [] as any[],
	selectedOrder: null as any,
});

const shippingMethodOptions = [
	{ label: '快递', value: 'Express' },
	{ label: '物流', value: 'Logistics' },
	{ label: '送货上门', value: 'Delivery' },
	{ label: '自提', value: 'Pickup' },
];

const rules = ref<FormRules>({
	orderId: [{required: true, message: '请选择销售订单！', trigger: 'change',},],
	warehouseId: [{required: true, message: '请选择仓库！', trigger: 'change',},],
});

onMounted(async () => {
	await loadWarehouseList();
});

const loadWarehouseList = async () => {
	const res = await getWarehouseList();
	// 分页接口返回结构: res.data.result.items
	state.warehouseList = res.data.result?.items || res.data.result || [];
};

const loadOrderList = async () => {
	const res = await getOrderList({ status: 'Approved' });
	// 分页接口返回结构: res.data.result.items
	state.orderList = res.data.result?.items || res.data.result || [];
};

const openDialog = async (mode: string = 'create') => {
	state.mode = mode;
	if (mode === 'create') {
		state.title = '从订单创建销售出库单';
		await loadOrderList();
		state.ruleForm = {
			orderId: null,
			warehouseId: null,
			shippingMethod: 'Express',
			trackingNo: '',
			remark: '',
		};
	} else {
		state.title = '手工创建销售出库单';
		state.ruleForm = {
			outboundNo: '',
			customerId: null,
			customerName: '',
			warehouseId: null,
			warehouseName: '',
			outboundDate: new Date(),
			shippingMethod: 'Express',
			trackingNo: '',
			totalQuantity: 0,
			totalAmount: 0,
			remark: '',
		};
	}
	state.showDialog = true;
};

const handleOrderChange = async (orderId: number) => {
	const order = state.orderList.find(o => o.id === orderId);
	if (order) {
		state.selectedOrder = order;
	}
};

const handleWarehouseChange = async (warehouseId: number) => {
	const warehouse = state.warehouseList.find(w => w.id === warehouseId);
	if (warehouse) {
		state.ruleForm.warehouseName = warehouse.warehouseName;
	}
};

const closeDialog = () => {
	emit("reloadTable");
	state.showDialog = false;
	state.selectedOrder = null;
};

const submit = async () => {
	ruleFormRef.value.validate(async (isValid: boolean, fields?: any) => {
		if (isValid) {
			state.loading = true;
			try {
				if (state.mode === 'create') {
					if (!state.selectedOrder) {
						ElMessage.error('请选择销售订单！');
						state.loading = false;
						return;
					}
					const params = {
						orderId: state.ruleForm.orderId,
						warehouseId: state.ruleForm.warehouseId,
						shippingMethod: state.ruleForm.shippingMethod,
						trackingNo: state.ruleForm.trackingNo,
						remark: state.ruleForm.remark,
					};
					const res = await salOutboundApi.createFromOrder(params);
					ElMessage.success('销售出库单创建成功！');
				} else {
					const res = await salOutboundApi.add(state.ruleForm);
					ElMessage.success('销售出库单创建成功！');
				}
				closeDialog();
			} catch (error: any) {
				ElMessage.error(error.message || '操作失败！');
			} finally {
				state.loading = false;
			}
		} else {
			ElMessage({
				message: `表单有${Object.keys(fields).length}处验证失败，请修改后再提交`,
				type: "error",
			});
		}
	});
};

defineExpose({ openDialog });
</script>
<template>
	<div class="salOutbound-edit-dialog">
		<el-dialog v-model="state.showDialog" :width="600" draggable :close-on-click-modal="false" :title="state.title">
			<el-form :model="state.ruleForm" ref="ruleFormRef" label-width="120px" :rules="rules">
				<template v-if="state.mode === 'create'">
					<el-form-item label="销售订单" prop="orderId">
						<el-select v-model="state.ruleForm.orderId" placeholder="请选择销售订单" filterable @change="handleOrderChange" style="width: 100%;">
							<el-option v-for="item in state.orderList" :key="item.id" :label="item.orderNo + ' - ' + item.customerName" :value="item.id" />
						</el-select>
					</el-form-item>
					<el-form-item label="仓库" prop="warehouseId">
						<el-select v-model="state.ruleForm.warehouseId" placeholder="请选择仓库" @change="handleWarehouseChange" style="width: 100%;">
							<el-option v-for="item in state.warehouseList" :key="item.id" :label="item.warehouseCode + ' - ' + item.warehouseName" :value="item.id" />
						</el-select>
					</el-form-item>
				</template>
				<template v-else>
					<el-row :gutter="20">
						<el-col :span="12">
							<el-form-item label="出库单号">
								<el-input v-model="state.ruleForm.outboundNo" placeholder="系统自动生成" :disabled="true" />
							</el-form-item>
						</el-col>
						<el-col :span="12">
							<el-form-item label="出库日期">
								<el-date-picker v-model="state.ruleForm.outboundDate" type="date" placeholder="选择日期" style="width: 100%;" />
							</el-form-item>
						</el-col>
					</el-row>
					<el-form-item label="客户">
						<el-input v-model="state.ruleForm.customerName" placeholder="请输入客户名称" />
					</el-form-item>
					<el-form-item label="仓库" prop="warehouseId">
						<el-select v-model="state.ruleForm.warehouseId" placeholder="请选择仓库" @change="handleWarehouseChange" style="width: 100%;">
							<el-option v-for="item in state.warehouseList" :key="item.id" :label="item.warehouseCode + ' - ' + item.warehouseName" :value="item.id" />
						</el-select>
					</el-form-item>
				</template>
				<el-form-item label="配送方式">
					<el-select v-model="state.ruleForm.shippingMethod" placeholder="请选择配送方式" style="width: 100%;">
						<el-option v-for="item in shippingMethodOptions" :key="item.value" :label="item.label" :value="item.value" />
					</el-select>
				</el-form-item>
				<el-form-item label="运单号">
					<el-input v-model="state.ruleForm.trackingNo" placeholder="请输入运单号" clearable />
				</el-form-item>
				<el-form-item label="备注">
					<el-input v-model="state.ruleForm.remark" placeholder="请输入备注" type="textarea" :rows="2" maxlength="500" show-word-limit />
				</el-form-item>
			</el-form>
			<template #footer>
				<span class="dialog-footer">
					<el-button @click="closeDialog">取 消</el-button>
					<el-button @click="submit" type="primary" v-reclick="1000" :loading="state.loading">确 定</el-button>
				</span>
			</template>
		</el-dialog>
	</div>
</template>
<style lang="scss" scoped>
:deep(.el-select), :deep(.el-input-number) {
  width: 100%;
}
</style>