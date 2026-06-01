<script lang="ts" name="purOrderItem" setup>
import { ref, reactive, onMounted } from "vue";
import { ElMessage } from "element-plus";
import type { FormRules } from "element-plus";
import { formatDate } from '/@/utils/formatTime';
import { usePurOrderItemApi } from '/@/api/purchase/purOrderItem';
import { usePurOrderApi } from '/@/api/purchase/purOrder';
import { getMaterialList } from '/@/api/base/basMaterial';

const emit = defineEmits(["reloadTable"]);
const purOrderItemApi = usePurOrderItemApi();
const purOrderApi = usePurOrderApi();
const ruleFormRef = ref();

const state = reactive({
	title: '',
	loading: false,
	showDialog: false,
	ruleForm: {} as any,
	stores: {},
	dropdownData: {} as any,
	orderOptions: [] as any[],
	materialOptions: [] as any[],
});

const rules = ref<FormRules>({
  orderId: [{required: true, message: '请选择采购订单！', trigger: 'blur',},],
  materialId: [{required: true, message: '请选择物料！', trigger: 'blur',},],
  orderQty: [{required: true, message: '请输入订单数量！', trigger: 'blur',},],
  unitName: [{required: true, message: '请输入单位名称！', trigger: 'blur',},],
});

onMounted(async () => {
	await loadOrderOptions();
	await loadMaterialOptions();
});

const loadOrderOptions = async () => {
	const result = await purOrderApi.list().then(res => res.data.result);
	state.orderOptions = result?.map((item: any) => ({
		label: `${item.orderNo}`,
		value: item.id,
		orderNo: item.orderNo,
	})) ?? [];
};

const loadMaterialOptions = async () => {
	const result = await getMaterialList().then(res => res.data.result);
	state.materialOptions = result?.map((item: any) => ({
		label: `${item.materialCode} - ${item.materialName}`,
		value: item.id,
		materialCode: item.materialCode,
		materialName: item.materialName,
		spec: item.spec,
		unit: item.unit,
	})) ?? [];
};

const handleMaterialChange = (materialId: number) => {
	const material = state.materialOptions.find(m => m.value === materialId);
	if (material) {
		state.ruleForm.materialCode = material.materialCode;
		state.ruleForm.materialName = material.materialName;
		state.ruleForm.spec = material.spec;
		state.ruleForm.unitName = material.unit;
	}
};

const handleOrderChange = (orderId: number) => {
	const order = state.orderOptions.find(o => o.value === orderId);
	if (order) {
		state.ruleForm.orderNo = order.orderNo;
	}
};

const openDialog = async (row: any, title: string) => {
	state.title = title;
	state.showDialog = true;
	state.loading = true;
	
	if (row?.id) {
		state.ruleForm = JSON.parse(JSON.stringify(row));
	} else {
		state.ruleForm = {
			id: null,
			orderId: null,
			materialId: null,
			materialCode: '',
			materialName: '',
			spec: '',
			unitId: null,
			unitName: '',
			orderQty: 0,
			price: 0,
			amount: 0,
			taxRate: 0,
			taxAmount: 0,
			deliveryDate: null,
			receivedQty: 0,
			remark: '',
			sortOrder: 0,
		};
	}
	
	state.loading = false;
};

const submit = async () => {
	ruleFormRef.value.validate(async (isValid: boolean, fields?: any) => {
		if (isValid) {
			state.loading = true;
			if (state.ruleForm.id) {
				await purOrderItemApi.update(state.ruleForm).then(res => {
					ElMessage.success('编辑成功');
					state.showDialog = false;
					emit('reloadTable');
				});
			} else {
				await purOrderItemApi.add(state.ruleForm).then(res => {
					ElMessage.success('新增成功');
					state.showDialog = false;
					emit('reloadTable');
				});
			}
			state.loading = false;
		}
	});
};

defineExpose({ openDialog });
</script>
<template>
	<div class="purOrderItem-container">
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
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="采购订单" prop="orderId">
							<el-select v-model="state.ruleForm.orderId" placeholder="请选择采购订单" clearable filterable @change="handleOrderChange">
								<el-option v-for="item in state.orderOptions" :key="item.value" :label="item.label" :value="item.value" />
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="物料" prop="materialId">
							<el-select v-model="state.ruleForm.materialId" placeholder="请选择物料" clearable filterable @change="handleMaterialChange">
								<el-option v-for="item in state.materialOptions" :key="item.value" :label="item.label" :value="item.value" />
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="物料编码">
							<el-input v-model="state.ruleForm.materialCode" placeholder="自动获取" disabled />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="物料名称">
							<el-input v-model="state.ruleForm.materialName" placeholder="自动获取" disabled />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="规格型号">
							<el-input v-model="state.ruleForm.spec" placeholder="自动获取" disabled />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="单位" prop="unitName">
							<el-input v-model="state.ruleForm.unitName" placeholder="自动获取" disabled />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="订单数量" prop="orderQty">
							<el-input-number v-model="state.ruleForm.orderQty" placeholder="请输入订单数量" :precision="4" :min="0" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="单价(不含税)">
							<el-input-number v-model="state.ruleForm.price" placeholder="请输入单价" :precision="4" :min="0" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="金额(不含税)">
							<el-input-number v-model="state.ruleForm.amount" placeholder="自动计算" :precision="4" :min="0" disabled />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="税率">
							<el-input-number v-model="state.ruleForm.taxRate" placeholder="请输入税率" :precision="2" :min="0" :max="100" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="税额">
							<el-input-number v-model="state.ruleForm.taxAmount" placeholder="自动计算" :precision="4" :min="0" disabled />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="交货日期">
							<el-date-picker v-model="state.ruleForm.deliveryDate" type="date" placeholder="请选择交货日期" value-format="YYYY-MM-DD" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="已收货数量">
							<el-input-number v-model="state.ruleForm.receivedQty" placeholder="请输入已收货数量" :precision="4" :min="0" disabled />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="备注">
							<el-input v-model="state.ruleForm.remark" placeholder="请输入备注" maxlength="200" show-word-limit clearable type="textarea" :rows="2" />
						</el-form-item>
					</el-col>
				</el-row>
			</el-form>
			<template #footer>
				<span class="dialog-footer">
					<el-button @click="() => state.showDialog = false">取 消</el-button>
					<el-button @click="submit" type="primary" v-reclick="1000">确 定</el-button>
				</span>
			</template>
		</el-dialog>
	</div>
</template>
<style scoped>
:deep(.el-input), :deep(.el-select), :deep(.el-input-number), :deep(.el-date-picker) {
  width: 100%;
}
</style>