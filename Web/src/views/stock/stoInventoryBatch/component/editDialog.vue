<script lang="ts" name="stoInventoryBatch" setup>
import { ref, reactive, onMounted } from "vue";
import { ElMessage } from "element-plus";
import type { FormRules } from "element-plus";
import { formatDate } from '/@/utils/formatTime';
import { useStoInventoryBatchApi } from '/@/api/stock/stoInventoryBatch';
//父级传递来的函数，用于回调
const emit = defineEmits(["reloadTable"]);
const stoInventoryBatchApi = useStoInventoryBatchApi();
const ruleFormRef = ref();

const state = reactive({
	title: '',
	loading: false,
	showDialog: false,
	ruleForm: {} as any,
	stores: {},
	dropdownData: {} as any,
});

// 自行添加其他规则
const rules = ref<FormRules>({
  warehouseId: [{required: true, message: '请选择仓库ID！', trigger: 'blur',},],
  warehouseCode: [{required: true, message: '请选择仓库编码！', trigger: 'blur',},],
  materialId: [{required: true, message: '请选择物料ID！', trigger: 'blur',},],
  materialCode: [{required: true, message: '请选择物料编码！', trigger: 'blur',},],
  batchNo: [{required: true, message: '请选择批号！', trigger: 'blur',},],
  stockQuantity: [{required: true, message: '请选择批次库存数量！', trigger: 'blur',},],
  frozenQuantity: [{required: true, message: '请选择冻结数量！', trigger: 'blur',},],
  costPrice: [{required: true, message: '请选择批次成本价！', trigger: 'blur',},],
});

// 页面加载时
onMounted(async () => {
});

// 打开弹窗
const openDialog = async (row: any, title: string) => {
	state.title = title;
	row = row ?? {  };
	state.ruleForm = row.id ? await stoInventoryBatchApi.detail(row.id).then(res => res.data.result) : JSON.parse(JSON.stringify(row));
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
			await stoInventoryBatchApi[state.ruleForm.id ? 'update' : 'add'](values);
			closeDialog();
		} else {
			ElMessage({
				message: `表单有${Object.keys(fields).length}处验证失败，请修改后再提交`,
				type: "error",
			});
		}
	});
};

//将属性或者函数暴露给父组件
defineExpose({ openDialog });
</script>
<template>
	<div class="stoInventoryBatch-container">
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
						<el-form-item label="仓库ID" prop="warehouseId">
							<el-input v-model="state.ruleForm.warehouseId" placeholder="请输入仓库ID" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="仓库编码" prop="warehouseCode">
							<el-input v-model="state.ruleForm.warehouseCode" placeholder="请输入仓库编码" maxlength="50" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="库位编码" prop="locationCode">
							<el-input v-model="state.ruleForm.locationCode" placeholder="请输入库位编码" maxlength="50" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="物料ID" prop="materialId">
							<el-input v-model="state.ruleForm.materialId" placeholder="请输入物料ID" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="物料编码" prop="materialCode">
							<el-input v-model="state.ruleForm.materialCode" placeholder="请输入物料编码" maxlength="50" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="批号" prop="batchNo">
							<el-input v-model="state.ruleForm.batchNo" placeholder="请输入批号" maxlength="50" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="有效期" prop="expiryDate">
							<el-date-picker v-model="state.ruleForm.expiryDate" type="date" placeholder="有效期" />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="批次库存数量" prop="stockQuantity">
							<el-input-number v-model="state.ruleForm.stockQuantity" placeholder="请输入批次库存数量" clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="冻结数量" prop="frozenQuantity">
							<el-input-number v-model="state.ruleForm.frozenQuantity" placeholder="请输入冻结数量" clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="批次成本价" prop="costPrice">
							<el-input-number v-model="state.ruleForm.costPrice" placeholder="请输入批次成本价" clearable />
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