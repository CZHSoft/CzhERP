<script lang="ts" name="stoCountItem" setup>
import { ref, reactive, onMounted, watch } from "vue";
import { ElMessage } from "element-plus";
import type { FormRules } from "element-plus";
import { formatDate } from '/@/utils/formatTime';
import { useStoCountItemApi } from '/@/api/stock/stoCountItem';
import { getCountList } from '/@/api/stock/stoCount';
import { getMaterialList } from '/@/api/base/basMaterial';

const emit = defineEmits(["reloadTable"]);
const stoCountItemApi = useStoCountItemApi();
const ruleFormRef = ref();

const state = reactive({
	title: '',
	loading: false,
	showDialog: false,
	ruleForm: {} as any,
	stores: {},
	dropdownData: {
		countList: [] as any[],
		materialList: [] as any[],
	},
});

// 表单验证规则
const rules = ref<FormRules>({
  countNo: [{required: true, message: '请选择盘点单！', trigger: 'change',},],
  materialName: [{required: true, message: '请选择物料！', trigger: 'change',},],
  actualQuantity: [{required: true, message: '请输入实际数量！', trigger: 'blur',},],
});

// 页面加载时
onMounted(async () => {
	await Promise.all([loadCountList(), loadMaterialList()]);
});

// 加载盘点单列表
const loadCountList = async () => {
	const res = await getCountList({ status: 'Draft' });
	state.dropdownData.countList = res.data.result.items || [];
};

// 加载物料列表
const loadMaterialList = async () => {
	const res = await getMaterialList();
	state.dropdownData.materialList = res.data.result || [];
};

// 盘点单选择变更
const handleCountChange = (item: any) => {
	if (item) {
		state.ruleForm.countId = item.id;
		state.ruleForm.countNo = item.countNo;
		state.ruleForm.warehouseId = item.warehouseId;
		state.ruleForm.warehouseCode = item.warehouseCode;
		state.ruleForm.warehouseName = item.warehouseName;
	} else {
		state.ruleForm.countId = '';
		state.ruleForm.countNo = '';
		state.ruleForm.warehouseId = '';
		state.ruleForm.warehouseCode = '';
		state.ruleForm.warehouseName = '';
	}
};

// 物料选择变更
const handleMaterialChange = (item: any) => {
	if (item) {
		state.ruleForm.materialId = item.id;
		state.ruleForm.materialCode = item.materialCode;
		state.ruleForm.materialName = item.materialName;
		state.ruleForm.spec = item.spec;
		state.ruleForm.unit = item.unit;
	} else {
		state.ruleForm.materialId = '';
		state.ruleForm.materialCode = '';
		state.ruleForm.materialName = '';
		state.ruleForm.spec = '';
		state.ruleForm.unit = '';
	}
};

// 监听实际数量变化，自动计算差异
watch(() => state.ruleForm.actualQuantity, (newVal) => {
	calculateDiff();
});

watch(() => state.ruleForm.systemQuantity, (newVal) => {
	calculateDiff();
});

// 计算差异
const calculateDiff = () => {
	const systemQty = state.ruleForm.systemQuantity || 0;
	const actualQty = state.ruleForm.actualQuantity || 0;
	state.ruleForm.diffQuantity = actualQty - systemQty;
	
	const costPrice = state.ruleForm.costPrice || 0;
	state.ruleForm.diffAmount = state.ruleForm.diffQuantity * costPrice;
};

// 打开弹窗
const openDialog = async (row: any, title: string) => {
	state.title = title;
	row = row ?? {  };
	if (row.id) {
		state.ruleForm = await stoCountItemApi.detail(row.id).then(res => res.data.result);
	} else {
		state.ruleForm = JSON.parse(JSON.stringify(row));
		state.ruleForm.systemQuantity = 0;
		state.ruleForm.actualQuantity = 0;
		state.ruleForm.diffQuantity = 0;
		state.ruleForm.diffAmount = 0;
	}
	state.showDialog = true;
};

// 关闭弹窗
const closeDialog = () => {
	emit("reloadTable");
	state.showDialog = false;
};

// 提交
const submit = async () => {
	ruleFormRef.value.validate(async (isValid: boolean, fields?: any) => {
		if (isValid) {
			let values = state.ruleForm;
			await stoCountItemApi[state.ruleForm.id ? 'update' : 'add'](values);
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
	<div class="stoCountItem-container">
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
						<el-form-item label="盘点单" prop="countNo">
							<el-select 
								v-model="state.ruleForm.countNo" 
								placeholder="请选择盘点单" 
								clearable
								@change="(val) => handleCountChange(state.dropdownData.countList.find(c => c.countNo === val))">
								<el-option 
									v-for="item in state.dropdownData.countList" 
									:key="item.id" 
									:label="item.countNo" 
									:value="item.countNo" />
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="物料名称" prop="materialName">
							<el-select 
								v-model="state.ruleForm.materialName" 
								placeholder="请选择物料" 
								clearable
								@change="(val) => handleMaterialChange(state.dropdownData.materialList.find(m => m.materialName === val))">
								<el-option 
									v-for="item in state.dropdownData.materialList" 
									:key="item.id" 
									:label="item.materialName" 
									:value="item.materialName" />
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="规格型号" prop="spec">
							<el-input v-model="state.ruleForm.spec" placeholder="自动填充" :disabled="true" maxlength="100" show-word-limit />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="单位" prop="unit">
							<el-input v-model="state.ruleForm.unit" placeholder="自动填充" :disabled="true" maxlength="20" show-word-limit />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="系统数量" prop="systemQuantity">
							<el-input-number v-model="state.ruleForm.systemQuantity" placeholder="系统库存数量" clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="实际数量" prop="actualQuantity">
							<el-input-number v-model="state.ruleForm.actualQuantity" placeholder="实际盘点数量" clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="差异数量" prop="diffQuantity">
							<el-input-number v-model="state.ruleForm.diffQuantity" placeholder="自动计算" :disabled="true" clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="成本单价" prop="costPrice">
							<el-input-number v-model="state.ruleForm.costPrice" placeholder="成本单价" :precision="2" clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="差异金额" prop="diffAmount">
							<el-input-number v-model="state.ruleForm.diffAmount" placeholder="自动计算" :disabled="true" :precision="2" clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="批号" prop="batchNo">
							<el-input v-model="state.ruleForm.batchNo" placeholder="请输入批号" maxlength="50" show-word-limit clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="库位编码" prop="locationCode">
							<el-input v-model="state.ruleForm.locationCode" placeholder="请输入库位编码" maxlength="50" show-word-limit clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="排序号" prop="sortOrder">
							<el-input-number v-model="state.ruleForm.sortOrder" placeholder="请输入排序号" :min="1" clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="24" class="mb20" >
						<el-form-item label="备注" prop="remark">
							<el-input v-model="state.ruleForm.remark" placeholder="请输入备注" maxlength="200" show-word-limit clearable />
						</el-form-item>
					</el-col>
					<!-- 隐藏字段 -->
					<el-form-item v-show="false" prop="countId">
						<el-input v-model="state.ruleForm.countId" />
					</el-form-item>
					<el-form-item v-show="false" prop="materialId">
						<el-input v-model="state.ruleForm.materialId" />
					</el-form-item>
					<el-form-item v-show="false" prop="materialCode">
						<el-input v-model="state.ruleForm.materialCode" />
					</el-form-item>
					<el-form-item v-show="false" prop="warehouseId">
						<el-input v-model="state.ruleForm.warehouseId" />
					</el-form-item>
					<el-form-item v-show="false" prop="warehouseCode">
						<el-input v-model="state.ruleForm.warehouseCode" />
					</el-form-item>
					<el-form-item v-show="false" prop="warehouseName">
						<el-input v-model="state.ruleForm.warehouseName" />
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