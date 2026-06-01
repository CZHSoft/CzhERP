<script lang="ts" name="stoStockInItem" setup>
import { ref, reactive, onMounted } from "vue";
import { ElMessage } from "element-plus";
import type { FormRules } from "element-plus";
import { formatDate } from '/@/utils/formatTime';
import { useStoStockInItemApi, getStockInList, getMaterialList, getLocationList } from '/@/api/stock/stoStockInItem';

const emit = defineEmits(["reloadTable"]);
const stoStockInItemApi = useStoStockInItemApi();
const ruleFormRef = ref();

const state = reactive({
	title: '',
	loading: false,
	showDialog: false,
	ruleForm: {} as any,
	stores: {},
	dropdownData: {} as any,
	stockInList: [] as any[],
	materialList: [] as any[],
	locationList: [] as any[],
});

const rules = ref<FormRules>({
  stockInId: [{required: true, message: '请选择入库单！', trigger: 'change',},],
  materialId: [{required: true, message: '请选择物料！', trigger: 'change',},],
  materialCode: [{required: true, message: '请选择物料！', trigger: 'change',},],
  materialName: [{required: true, message: '请选择物料！', trigger: 'change',},],
  unit: [{required: true, message: '请输入单位！', trigger: 'blur',},],
  quantity: [{required: true, message: '请输入入库数量！', trigger: 'blur',},],
  unitPrice: [{required: true, message: '请输入单价！', trigger: 'blur',},],
  amount: [{required: true, message: '请输入金额！', trigger: 'blur',},],
  sortOrder: [{required: true, message: '请输入排序号！', trigger: 'blur',},],
});

onMounted(async () => {
	await loadStockInList();
	await loadMaterialList();
});

const loadStockInList = async () => {
	const res = await getStockInList();
	state.stockInList = res.data.result || [];
};

const loadMaterialList = async () => {
	const res = await getMaterialList();
	state.materialList = res.data.result || [];
};

const loadLocationList = async (warehouseId?: number) => {
	if (warehouseId) {
		const res = await getLocationList(warehouseId);
		state.locationList = res.data.result || [];
	} else {
		const res = await getLocationList();
		state.locationList = res.data.result || [];
	}
};

const handleStockInChange = (stockInId: number) => {
	const stockIn = state.stockInList.find((s: any) => s.id === stockInId);
	if (stockIn) {
		state.ruleForm.warehouseId = stockIn.warehouseId;
		state.ruleForm.warehouseCode = stockIn.warehouseCode;
		loadLocationList(stockIn.warehouseId);
	}
};

const handleMaterialChange = (materialId: number) => {
	const material = state.materialList.find((m: any) => m.id === materialId);
	if (material) {
		state.ruleForm.materialId = material.id;
		state.ruleForm.materialCode = material.materialCode;
		state.ruleForm.materialName = material.materialName;
		state.ruleForm.spec = material.spec || '';
		state.ruleForm.unit = material.unit || '';
		state.ruleForm.unitPrice = material.costPrice || 0;
	}
};

const calculateAmount = () => {
	if (state.ruleForm.quantity && state.ruleForm.unitPrice) {
		state.ruleForm.amount = state.ruleForm.quantity * state.ruleForm.unitPrice;
	}
};

const openDialog = async (row: any, title: string) => {
	state.title = title;
	row = row ?? {  };
	if (row.id) {
		state.ruleForm = await stoStockInItemApi.detail(row.id).then(res => res.data.result);
		if (state.ruleForm.stockInId) {
			loadLocationList();
		}
	} else {
		state.ruleForm = JSON.parse(JSON.stringify(row));
		state.ruleForm.quantity = 0;
		state.ruleForm.unitPrice = 0;
		state.ruleForm.amount = 0;
		state.ruleForm.sortOrder = 1;
	}
	state.showDialog = true;
};

const closeDialog = () => {
	emit("reloadTable");
	state.showDialog = false;
};

const submit = async () => {
	ruleFormRef.value.validate(async (isValid: boolean, fields?: any) => {
		if (isValid) {
			let values = state.ruleForm;
			await stoStockInItemApi[state.ruleForm.id ? 'update' : 'add'](values);
			closeDialog();
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
	<div class="stoStockInItem-container">
		<el-dialog v-model="state.showDialog" :width="900" draggable :close-on-click-modal="false">
			<template #header>
				<div style="color: #fff">
					<span>{{ state.title }}</span>
				</div>
			</template>
			<el-form :model="state.ruleForm" ref="ruleFormRef" label-width="auto" :rules="rules">
				<el-row :gutter="35">
					<el-form-item v-show="false">
						<el-input v-model="state.ruleForm.id" />
					</el-form-item>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="入库单" prop="stockInId">
							<el-select v-model="state.ruleForm.stockInId" placeholder="请选择入库单" @change="handleStockInChange">
								<el-option v-for="item in state.stockInList" :key="item.id" :label="item.stockInNo + ' - ' + item.stockInDate" :value="item.id" />
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="物料" prop="materialId">
							<el-select v-model="state.ruleForm.materialId" placeholder="请选择物料" @change="handleMaterialChange">
								<el-option v-for="item in state.materialList" :key="item.id" :label="item.materialCode + ' - ' + item.materialName" :value="item.id" />
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="物料编码" prop="materialCode">
							<el-input v-model="state.ruleForm.materialCode" placeholder="根据物料自动填充" :disabled="true" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="物料名称" prop="materialName">
							<el-input v-model="state.ruleForm.materialName" placeholder="根据物料自动填充" :disabled="true" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="规格型号" prop="spec">
							<el-input v-model="state.ruleForm.spec" placeholder="根据物料自动填充" :disabled="true" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="单位" prop="unit">
							<el-input v-model="state.ruleForm.unit" placeholder="请输入单位" maxlength="20" show-word-limit clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="入库数量" prop="quantity">
							<el-input-number v-model="state.ruleForm.quantity" placeholder="请输入入库数量" clearable @change="calculateAmount" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="单价" prop="unitPrice">
							<el-input-number v-model="state.ruleForm.unitPrice" placeholder="请输入单价" clearable @change="calculateAmount" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="金额" prop="amount">
							<el-input-number v-model="state.ruleForm.amount" placeholder="系统自动计算" :disabled="true" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="入库库位" prop="locationCode">
							<el-select v-model="state.ruleForm.locationCode" placeholder="请选择库位" clearable>
								<el-option v-for="item in state.locationList" :key="item.id" :label="item.locationCode + ' - ' + item.locationName" :value="item.locationCode" />
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="批号" prop="batchNo">
							<el-input v-model="state.ruleForm.batchNo" placeholder="请输入批号" maxlength="50" show-word-limit clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="有效期" prop="expiryDate">
							<el-date-picker v-model="state.ruleForm.expiryDate" type="date" placeholder="有效期" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="排序号" prop="sortOrder">
							<el-input-number v-model="state.ruleForm.sortOrder" placeholder="请输入排序号" clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="备注" prop="remark">
							<el-input v-model="state.ruleForm.remark" placeholder="请输入备注" maxlength="200" show-word-limit clearable />
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
<style lang="scss" scoped>
:deep(.el-select), :deep(.el-input-number) {
  width: 100%;
}
</style>