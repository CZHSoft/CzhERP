<script lang="ts" name="purRequisitionItem" setup>
import { ref, reactive, onMounted } from "vue";
import { ElMessage } from "element-plus";
import type { FormRules } from "element-plus";
import { formatDate } from '/@/utils/formatTime';
import { usePurRequisitionItemApi } from '/@/api/purchase/purRequisitionItem';
import { usePurRequisitionApi } from '/@/api/purchase/purRequisition';
import { getMaterialList } from '/@/api/base/basMaterial';

const emit = defineEmits(["reloadTable"]);
const purRequisitionItemApi = usePurRequisitionItemApi();
const purRequisitionApi = usePurRequisitionApi();
const ruleFormRef = ref();

const state = reactive({
	title: '',
	loading: false,
	showDialog: false,
	ruleForm: {} as any,
	stores: {},
	dropdownData: {} as any,
	requisitionOptions: [] as any[],
	materialOptions: [] as any[],
});

const rules = ref<FormRules>({
  requisitionId: [{required: true, message: '请选择采购申请单！', trigger: 'blur',},],
  materialId: [{required: true, message: '请选择物料！', trigger: 'blur',},],
  requestQty: [{required: true, message: '请输入申请数量！', trigger: 'blur',},],
  unitName: [{required: true, message: '请输入单位名称！', trigger: 'blur',},],
});

onMounted(async () => {
	await loadRequisitionOptions();
	await loadMaterialOptions();
});

const loadRequisitionOptions = async () => {
	const result = await purRequisitionApi.list().then(res => res.data.result);
	state.requisitionOptions = result?.map((item: any) => ({
		label: `${item.requisitionNo} - ${item.purpose || ''}`,
		value: item.id,
		requisitionNo: item.requisitionNo,
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

const handleRequisitionChange = (requisitionId: number) => {
	const requisition = state.requisitionOptions.find(r => r.value === requisitionId);
	if (requisition) {
		state.ruleForm.requisitionNo = requisition.requisitionNo;
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
			requisitionId: null,
			materialId: null,
			materialCode: '',
			materialName: '',
			spec: '',
			unitId: null,
			unitName: '',
			requestQty: 0,
			expectedPrice: null,
			amount: 0,
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
				await purRequisitionItemApi.update(state.ruleForm).then(res => {
					ElMessage.success('编辑成功');
					state.showDialog = false;
					emit('reloadTable');
				});
			} else {
				await purRequisitionItemApi.add(state.ruleForm).then(res => {
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
	<div class="purRequisitionItem-container">
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
						<el-form-item label="采购申请单" prop="requisitionId">
							<el-select v-model="state.ruleForm.requisitionId" placeholder="请选择采购申请单" clearable filterable @change="handleRequisitionChange">
								<el-option v-for="item in state.requisitionOptions" :key="item.value" :label="item.label" :value="item.value" />
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
						<el-form-item label="申请数量" prop="requestQty">
							<el-input-number v-model="state.ruleForm.requestQty" placeholder="请输入申请数量" :precision="4" :min="0" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="期望单价">
							<el-input-number v-model="state.ruleForm.expectedPrice" placeholder="请输入期望单价" :precision="4" :min="0" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="金额">
							<el-input-number v-model="state.ruleForm.amount" placeholder="自动计算" :precision="4" :min="0" disabled />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="排序">
							<el-input-number v-model="state.ruleForm.sortOrder" placeholder="请输入排序" :min="0" />
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
:deep(.el-input), :deep(.el-select), :deep(.el-input-number) {
  width: 100%;
}
</style>