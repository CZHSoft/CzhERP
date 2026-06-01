<script lang="ts" name="purOrderInboundDialog" setup>
import { ref, reactive, onMounted } from "vue";
import { ElMessage } from "element-plus";
import type { FormRules } from "element-plus";
import { usePurInboundApi } from '/@/api/purchase/purInbound';
import { usePurOrderApi } from '/@/api/purchase/purOrder';
import { getWarehouseList } from '/@/api/stock/stoStockIn';

const emit = defineEmits(["reloadTable"]);
const purInboundApi = usePurInboundApi();
const purOrderApi = usePurOrderApi();
const ruleFormRef = ref();

const state = reactive({
	title: '从采购订单创建入库单',
	loading: false,
	showDialog: false,
	ruleForm: {} as any,
	warehouseOptions: [] as any[],
	orderOptions: [] as any[],
});

const rules = ref<FormRules>({
  purOrderId: [{required: true, message: '请选择采购订单！', trigger: 'change',}],
  warehouseId: [{required: true, message: '请选择仓库！', trigger: 'change',}],
});

onMounted(async () => {
	await loadWarehouseOptions();
});

const loadWarehouseOptions = async () => {
	try {
		const res = await getWarehouseList();
		state.warehouseOptions = res.data.result || [];
	} catch (e) {
		console.error('加载仓库列表失败', e);
		state.warehouseOptions = [];
	}
};

const loadOrderOptions = async () => {
	try {
		const res = await purOrderApi.list().then(res => res.data.result);
		state.orderOptions = res?.map((item: any) => ({
			label: `${item.orderNo} - ${item.supplierName || ''}`,
			value: item.id,
			orderNo: item.orderNo,
			supplierName: item.supplierName,
		})) ?? [];
	} catch (e) {
		console.error('加载采购订单列表失败', e);
		state.orderOptions = [];
	}
};

const handleWarehouseChange = (warehouseId: number) => {
	const warehouse = state.warehouseOptions.find((w: any) => w.id === warehouseId);
	if (warehouse) {
		state.ruleForm.warehouseCode = warehouse.warehouseCode;
		state.ruleForm.warehouseName = warehouse.warehouseName;
	}
};

const openDialog = async () => {
	await loadOrderOptions();
	state.ruleForm = {
		purOrderId: null,
		warehouseId: null,
		warehouseCode: '',
		warehouseName: '',
		arrivalDate: null,
		batchNo: '',
		remark: '',
	};
	state.showDialog = true;
};

const closeDialog = () => {
	emit("reloadTable");
	state.showDialog = false;
};

const submit = async () => {
	ruleFormRef.value.validate(async (isValid: boolean, fields?: any) => {
		if (isValid) {
			state.loading = true;
			try {
				const params = {
					purOrderId: state.ruleForm.purOrderId,
					warehouseId: state.ruleForm.warehouseId,
					warehouseCode: state.ruleForm.warehouseCode,
					warehouseName: state.ruleForm.warehouseName,
					arrivalDate: state.ruleForm.arrivalDate,
					batchNo: state.ruleForm.batchNo,
					remark: state.ruleForm.remark,
				};

				const res = await purInboundApi.createFromPurOrder(params);
				ElMessage.success(res.data.result?.message || '入库单创建成功！');
				closeDialog();
			} catch (error: any) {
				ElMessage.error('创建失败：' + (error.message || '未知错误'));
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
	<div class="purOrderInboundDialog-container">
		<el-dialog v-model="state.showDialog" :width="600" draggable :close-on-click-modal="false">
			<template #header>
				<div style="color: #fff">
					<span>{{ state.title }}</span>
				</div>
			</template>
			<el-form :model="state.ruleForm" ref="ruleFormRef" label-width="auto" :rules="rules" v-loading="state.loading">
				<el-row :gutter="35">
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="采购订单" prop="purOrderId">
							<el-select v-model="state.ruleForm.purOrderId" placeholder="请选择采购订单" clearable filterable>
								<el-option v-for="item in state.orderOptions" :key="item.value" :label="item.label" :value="item.value" />
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="仓库" prop="warehouseId">
							<el-select v-model="state.ruleForm.warehouseId" placeholder="请选择仓库" clearable @change="handleWarehouseChange">
								<el-option v-for="item in state.warehouseOptions" :key="item.id" :label="`${item.warehouseCode} - ${item.warehouseName}`" :value="item.id" />
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="仓库编码">
							<el-input v-model="state.ruleForm.warehouseCode" placeholder="根据仓库自动填充" :disabled="true" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="仓库名称">
							<el-input v-model="state.ruleForm.warehouseName" placeholder="根据仓库自动填充" :disabled="true" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="到货日期">
							<el-date-picker v-model="state.ruleForm.arrivalDate" type="date" placeholder="请选择到货日期（可选）" value-format="YYYY-MM-DD" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="批号">
							<el-input v-model="state.ruleForm.batchNo" placeholder="请输入批号（可选）" clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="备注">
							<el-input v-model="state.ruleForm.remark" placeholder="请输入备注（可选）" maxlength="500" show-word-limit clearable type="textarea" :rows="3" />
						</el-form-item>
					</el-col>
				</el-row>
			</el-form>
			<template #footer>
				<span class="dialog-footer">
					<el-button @click="closeDialog">取 消</el-button>
					<el-button @click="submit" type="primary" v-reclick="1000" :loading="state.loading">创 建</el-button>
				</span>
			</template>
		</el-dialog>
	</div>
</template>

<style lang="scss" scoped>
:deep(.el-select), :deep(.el-input-number), :deep(.el-date-picker) {
  width: 100%;
}
</style>