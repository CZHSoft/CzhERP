<script lang="ts" name="stoAdjustItem" setup>
import { ref, reactive, onMounted } from "vue";
import { ElMessage } from "element-plus";
import type { FormRules } from "element-plus";
import { formatDate } from '/@/utils/formatTime';
import { useStoAdjustItemApi } from '/@/api/stock/stoAdjustItem';
import { getMaterialList } from '/@/api/base/basMaterial';
import { getAdjustList } from '/@/api/stock/stoAdjust';

const emit = defineEmits(["reloadTable"]);
const stoAdjustItemApi = useStoAdjustItemApi();
const ruleFormRef = ref();

const state = reactive({
	title: '',
	loading: false,
	showDialog: false,
	ruleForm: {} as any,
	stores: {},
	dropdownData: {
		materialList: [] as any[],
		adjustList: [] as any[],
	},
});

const rules = ref<FormRules>({
  adjustId: [{required: true, message: '请选择调整单！', trigger: 'change',}],
  materialName: [{required: true, message: '请选择物料！', trigger: 'change',}],
  unit: [{required: true, message: '请输入单位！', trigger: 'blur',}],
  adjustQuantity: [{required: true, message: '请输入调整数量！', trigger: 'blur',}],
  costPrice: [{required: true, message: '请输入成本单价！', trigger: 'blur',}],
});

onMounted(async () => {
	await loadMaterialList();
	await loadAdjustList();
});

const loadMaterialList = async () => {
	const res = await getMaterialList();
	state.dropdownData.materialList = res.data.result || [];
};

const loadAdjustList = async () => {
	const res = await getAdjustList();
	state.dropdownData.adjustList = res.data.result.items || [];
};

const handleMaterialChange = (item: any) => {
	if (item) {
		state.ruleForm.materialId = item.id;
		state.ruleForm.materialCode = item.materialCode;
		state.ruleForm.materialName = item.materialName;
		state.ruleForm.spec = item.spec || '';
		state.ruleForm.unit = item.unit || '';
		state.ruleForm.costPrice = item.costPrice || 0;
	} else {
		state.ruleForm.materialId = '';
		state.ruleForm.materialCode = '';
		state.ruleForm.materialName = '';
		state.ruleForm.spec = '';
		state.ruleForm.unit = '';
		state.ruleForm.costPrice = 0;
	}
	calculateAmount();
};

const handleAdjustChange = (item: any) => {
	if (item) {
		state.ruleForm.adjustId = item.id;
	} else {
		state.ruleForm.adjustId = '';
	}
};

const calculateAmount = () => {
	const quantity = state.ruleForm.adjustQuantity || 0;
	const price = state.ruleForm.costPrice || 0;
	state.ruleForm.adjustAmount = quantity * price;
};

const openDialog = async (row: any, title: string) => {
	state.title = title;
	row = row ?? {  };
	if (row.id) {
		state.ruleForm = await stoAdjustItemApi.detail(row.id).then(res => res.data.result);
	} else {
		state.ruleForm = JSON.parse(JSON.stringify(row));
		state.ruleForm.sortOrder = 0;
		state.ruleForm.adjustQuantity = 0;
		state.ruleForm.costPrice = 0;
		state.ruleForm.adjustAmount = 0;
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
			await stoAdjustItemApi[state.ruleForm.id ? 'update' : 'add'](values);
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
	<div class="stoAdjustItem-container">
		<el-dialog v-model="state.showDialog" :width="800" draggable :close-on-click-modal="false">
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
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="调整单" prop="adjustId">
							<el-select 
								v-model="state.ruleForm.adjustId" 
								placeholder="请选择调整单" 
								clearable
								@change="(val) => handleAdjustChange(state.dropdownData.adjustList.find(a => a.id === val))">
								<el-option 
									v-for="item in state.dropdownData.adjustList" 
									:key="item.id" 
									:label="item.adjustNo + ' - ' + item.warehouseName" 
									:value="item.id" />
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="物料" prop="materialName">
							<el-select 
								v-model="state.ruleForm.materialName" 
								placeholder="请选择物料" 
								clearable
								filterable
								@change="(val) => handleMaterialChange(state.dropdownData.materialList.find(m => m.materialName === val))">
								<el-option 
									v-for="item in state.dropdownData.materialList" 
									:key="item.id" 
									:label="item.materialCode + ' - ' + item.materialName" 
									:value="item.materialName" />
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="规格型号" prop="spec">
							<el-input v-model="state.ruleForm.spec" placeholder="请输入规格型号" maxlength="100" show-word-limit clearable :disabled="true" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="单位" prop="unit">
							<el-input v-model="state.ruleForm.unit" placeholder="请输入单位" maxlength="20" show-word-limit clearable :disabled="true" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="调整数量" prop="adjustQuantity">
							<el-input-number v-model="state.ruleForm.adjustQuantity" placeholder="请输入调整数量(正数增加,负数减少)" clearable @change="calculateAmount" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="成本单价" prop="costPrice">
							<el-input-number v-model="state.ruleForm.costPrice" placeholder="请输入成本单价" clearable @change="calculateAmount" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="调整金额" prop="adjustAmount">
							<el-input-number v-model="state.ruleForm.adjustAmount" placeholder="调整金额" :disabled="true" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="库位编码" prop="locationCode">
							<el-input v-model="state.ruleForm.locationCode" placeholder="请输入库位编码" maxlength="50" show-word-limit clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="批号" prop="batchNo">
							<el-input v-model="state.ruleForm.batchNo" placeholder="请输入批号" maxlength="50" show-word-limit clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20" >
						<el-form-item label="备注" prop="remark">
							<el-input v-model="state.ruleForm.remark" placeholder="请输入备注" maxlength="200" show-word-limit clearable />
						</el-form-item>
					</el-col>
					<el-form-item v-show="false" prop="materialId">
						<el-input v-model="state.ruleForm.materialId" />
					</el-form-item>
					<el-form-item v-show="false" prop="materialCode">
						<el-input v-model="state.ruleForm.materialCode" />
					</el-form-item>
					<el-form-item v-show="false" prop="sortOrder">
						<el-input-number v-model="state.ruleForm.sortOrder" />
					</el-form-item>
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