<script lang="ts" name="purInbound" setup>
import { ref, reactive, onMounted } from "vue";
import { ElMessage } from "element-plus";
import type { FormRules } from "element-plus";
import { formatDate } from '/@/utils/formatTime';
import { usePurInboundApi } from '/@/api/purchase/purInbound';
import { usePurOrderApi } from '/@/api/purchase/purOrder';
import { usePurSupplierApi } from '/@/api/purchase/purSupplier';
import { getWarehouseList } from '/@/api/stock/stoStockIn';

const emit = defineEmits(["reloadTable"]);
const purInboundApi = usePurInboundApi();
const purOrderApi = usePurOrderApi();
const purSupplierApi = usePurSupplierApi();
const ruleFormRef = ref();

const state = reactive({
	title: '',
	loading: false,
	showDialog: false,
	ruleForm: {} as any,
	orderOptions: [] as any[],
	supplierOptions: [] as any[],
	warehouseOptions: [] as any[],
	statusOptions: [
		{ label: '待质检', value: 0 },
		{ label: '质检中', value: 1 },
		{ label: '合格', value: 2 },
		{ label: '部分合格', value: 3 },
		{ label: '不合格', value: 4 },
	],
});

const rules = ref<FormRules>({
  orderId: [{required: true, message: '请选择采购订单！', trigger: 'change',}],
  supplierId: [{required: true, message: '请选择供应商！', trigger: 'change',}],
  warehouseId: [{required: true, message: '请选择仓库！', trigger: 'change',}],
  inboundDate: [{required: true, message: '请选择入库日期！', trigger: 'change',}],
  status: [{required: true, message: '请选择状态！', trigger: 'change',}],
});

onMounted(async () => {
	await loadOrderOptions();
	await loadSupplierOptions();
	await loadWarehouseOptions();
});

const loadOrderOptions = async () => {
	try {
		const res = await purOrderApi.list().then(res => res.data.result);
		state.orderOptions = res?.map((item: any) => ({
			label: `${item.orderNo} - ${item.supplierName || ''}`,
			value: item.id,
			orderNo: item.orderNo,
			supplierId: item.supplierId,
			supplierName: item.supplierName,
		})) ?? [];
	} catch (e) {
		console.error('加载采购订单列表失败', e);
		state.orderOptions = [];
	}
};

const loadSupplierOptions = async () => {
	try {
		const res = await purSupplierApi.page({ page: 1, pageSize: 9999 }).then(res => res.data.result);
		state.supplierOptions = res?.items?.map((item: any) => ({
			label: `${item.supplierCode} - ${item.supplierName}`,
			value: item.id,
			supplierCode: item.supplierCode,
			supplierName: item.supplierName,
		})) ?? [];
	} catch (e) {
		console.error('加载供应商列表失败', e);
		state.supplierOptions = [];
	}
};

const loadWarehouseOptions = async () => {
	try {
		const res = await getWarehouseList();
		state.warehouseOptions = res.data.result || [];
	} catch (e) {
		console.error('加载仓库列表失败', e);
		state.warehouseOptions = [];
	}
};

const handleOrderChange = (orderId: number) => {
	const order = state.orderOptions.find((o: any) => o.value === orderId);
	if (order) {
		state.ruleForm.orderNo = order.orderNo;
		state.ruleForm.supplierId = order.supplierId;
		state.ruleForm.supplierName = order.supplierName;
	}
};

const handleSupplierChange = (supplierId: number) => {
	const supplier = state.supplierOptions.find((s: any) => s.value === supplierId);
	if (supplier) {
		state.ruleForm.supplierCode = supplier.supplierCode;
		state.ruleForm.supplierName = supplier.supplierName;
	}
};

const handleWarehouseChange = (warehouseId: number) => {
	const warehouse = state.warehouseOptions.find((w: any) => w.id === warehouseId);
	if (warehouse) {
		state.ruleForm.warehouseCode = warehouse.warehouseCode;
		state.ruleForm.warehouseName = warehouse.warehouseName;
	}
};

const openDialog = async (row: any, title: string) => {
	state.title = title;
	state.loading = true;
	state.showDialog = true;
	
	if (row?.id) {
		state.ruleForm = JSON.parse(JSON.stringify(row));
	} else {
		state.ruleForm = {
			id: null,
			inboundNo: '',
			orderId: null,
			orderNo: '',
			supplierId: null,
			supplierCode: '',
			supplierName: '',
			warehouseId: null,
			warehouseCode: '',
			warehouseName: '',
			inboundDate: formatDate(new Date(), 'YYYY-mm-dd'),
			arrivalDate: null,
			totalQty: 0,
			totalAmount: 0,
			status: 0,
			remark: '',
		};
		const nextCode = await purInboundApi.getNextCode().then(res => res.data.result);
		state.ruleForm.inboundNo = nextCode;
	}
	
	state.loading = false;
};

const submit = async () => {
	ruleFormRef.value.validate(async (isValid: boolean, fields?: any) => {
		if (isValid) {
			state.loading = true;
			if (state.ruleForm.id) {
				await purInboundApi.update(state.ruleForm).then(res => {
					ElMessage.success('编辑成功');
					state.showDialog = false;
					emit('reloadTable');
				});
			} else {
				await purInboundApi.add(state.ruleForm).then(res => {
					ElMessage.success('新增成功');
					state.showDialog = false;
					emit('reloadTable');
				});
			}
			state.loading = false;
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
	<div class="purInbound-container">
		<el-dialog v-model="state.showDialog" :width="800" draggable :close-on-click-modal="false">
			<template #header>
				<div style="color: #fff">
					<span>{{ state.title }}</span>
				</div>
			</template>
			<el-form :model="state.ruleForm" ref="ruleFormRef" label-width="auto" :rules="rules" v-loading="state.loading">
				<el-row :gutter="35">
					<el-form-item v-show="false">
						<el-input v-model="state.ruleForm.id" />
					</el-form-item>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="入库单号">
							<el-input v-model="state.ruleForm.inboundNo" placeholder="系统自动生成" disabled />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="采购订单" prop="orderId">
							<el-select v-model="state.ruleForm.orderId" placeholder="请选择采购订单" clearable filterable @change="handleOrderChange">
								<el-option v-for="item in state.orderOptions" :key="item.value" :label="item.label" :value="item.value" />
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="采购订单号">
							<el-input v-model="state.ruleForm.orderNo" placeholder="根据采购订单自动填充" disabled />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="供应商" prop="supplierId">
							<el-select v-model="state.ruleForm.supplierId" placeholder="请选择供应商" clearable filterable @change="handleSupplierChange">
								<el-option v-for="item in state.supplierOptions" :key="item.value" :label="item.label" :value="item.value" />
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="供应商编码">
							<el-input v-model="state.ruleForm.supplierCode" placeholder="根据供应商自动填充" disabled />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="供应商名称">
							<el-input v-model="state.ruleForm.supplierName" placeholder="根据供应商自动填充" disabled />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="仓库" prop="warehouseId">
							<el-select v-model="state.ruleForm.warehouseId" placeholder="请选择仓库" clearable filterable @change="handleWarehouseChange">
								<el-option v-for="item in state.warehouseOptions" :key="item.id" :label="`${item.warehouseCode} - ${item.warehouseName}`" :value="item.id" />
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="仓库编码">
							<el-input v-model="state.ruleForm.warehouseCode" placeholder="根据仓库自动填充" disabled />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="仓库名称">
							<el-input v-model="state.ruleForm.warehouseName" placeholder="根据仓库自动填充" disabled />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="入库日期" prop="inboundDate">
							<el-date-picker v-model="state.ruleForm.inboundDate" type="date" placeholder="请选择入库日期" value-format="YYYY-MM-DD" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="到货日期">
							<el-date-picker v-model="state.ruleForm.arrivalDate" type="date" placeholder="请选择到货日期" value-format="YYYY-MM-DD" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="总数量">
							<el-input-number v-model="state.ruleForm.totalQty" placeholder="请输入总数量" :precision="4" :min="0" disabled />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="总金额">
							<el-input-number v-model="state.ruleForm.totalAmount" placeholder="请输入总金额" :precision="4" :min="0" disabled />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="状态" prop="status">
							<el-select v-model="state.ruleForm.status" placeholder="请选择状态" :disabled="!!state.ruleForm.id">
								<el-option v-for="item in state.statusOptions" :key="item.value" :label="item.label" :value="item.value" />
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="备注">
							<el-input v-model="state.ruleForm.remark" placeholder="请输入备注" maxlength="500" show-word-limit clearable type="textarea" :rows="3" />
						</el-form-item>
					</el-col>
				</el-row>
			</el-form>
			<template #footer>
				<span class="dialog-footer">
					<el-button @click="() => state.showDialog = false">取 消</el-button>
					<el-button @click="submit" type="primary" v-reclick="1000" :loading="state.loading">确 定</el-button>
				</span>
			</template>
		</el-dialog>
	</div>
</template>
<style lang="scss" scoped>
:deep(.el-input), :deep(.el-select), :deep(.el-input-number), :deep(.el-date-picker) {
  width: 100%;
}
</style>