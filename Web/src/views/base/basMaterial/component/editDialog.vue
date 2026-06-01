<script lang="ts" name="basMaterial" setup>
import { ref, reactive, onMounted } from "vue";
import { ElMessage } from "element-plus";
import type { FormRules } from "element-plus";
import { formatDate } from '/@/utils/formatTime';
import { useBasMaterialApi } from '/@/api/base/basMaterial';
//父级传递来的函数，用于回调
const emit = defineEmits(["reloadTable"]);
const basMaterialApi = useBasMaterialApi();
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
  materialCode: [{required: true, message: '请选择物料编码！', trigger: 'blur',},],
  materialName: [{required: true, message: '请选择物料名称！', trigger: 'blur',},],
  unit: [{required: true, message: '请选择单位！', trigger: 'blur',},],
  minStock: [{required: true, message: '请选择最低库存！', trigger: 'blur',},],
  maxStock: [{required: true, message: '请选择最高库存！', trigger: 'blur',},],
  costPrice: [{required: true, message: '请选择成本价！', trigger: 'blur',},],
  salePrice: [{required: true, message: '请选择销售价！', trigger: 'blur',},],
  taxRate: [{required: true, message: '请选择税率！', trigger: 'blur',},],
  isEnabled: [{required: true, message: '请选择是否启用！', trigger: 'blur',},],
  isBatchManage: [{required: true, message: '请选择是否批次管理！', trigger: 'blur',},],
  isExpiryManage: [{required: true, message: '请选择是否效期管理！', trigger: 'blur',},],
});

// 页面加载时
onMounted(async () => {
});

// 打开弹窗
const openDialog = async (row: any, title: string) => {
	state.title = title;
	row = row ?? {  };
	state.ruleForm = row.id ? await basMaterialApi.detail(row.id).then(res => res.data.result) : JSON.parse(JSON.stringify(row));
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
			await basMaterialApi[state.ruleForm.id ? 'update' : 'add'](values);
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
	<div class="basMaterial-container">
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
						<el-form-item label="物料编码" prop="materialCode">
							<el-input v-model="state.ruleForm.materialCode" placeholder="请输入物料编码" maxlength="50" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="物料名称" prop="materialName">
							<el-input v-model="state.ruleForm.materialName" placeholder="请输入物料名称" maxlength="100" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="规格型号" prop="spec">
							<el-input v-model="state.ruleForm.spec" placeholder="请输入规格型号" maxlength="100" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="单位" prop="unit">
							<el-input v-model="state.ruleForm.unit" placeholder="请输入单位" maxlength="20" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="物料分类ID" prop="categoryId">
							<el-input v-model="state.ruleForm.categoryId" placeholder="请输入物料分类ID" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="物料分类编码" prop="categoryCode">
							<el-input v-model="state.ruleForm.categoryCode" placeholder="请输入物料分类编码" maxlength="50" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="物料分类名称" prop="categoryName">
							<el-input v-model="state.ruleForm.categoryName" placeholder="请输入物料分类名称" maxlength="50" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="品牌" prop="brand">
							<el-input v-model="state.ruleForm.brand" placeholder="请输入品牌" maxlength="50" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="型号" prop="model">
							<el-input v-model="state.ruleForm.model" placeholder="请输入型号" maxlength="50" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="最低库存" prop="minStock">
							<el-input-number v-model="state.ruleForm.minStock" placeholder="请输入最低库存" clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="最高库存" prop="maxStock">
							<el-input-number v-model="state.ruleForm.maxStock" placeholder="请输入最高库存" clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="成本价" prop="costPrice">
							<el-input-number v-model="state.ruleForm.costPrice" placeholder="请输入成本价" clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="销售价" prop="salePrice">
							<el-input-number v-model="state.ruleForm.salePrice" placeholder="请输入销售价" clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="税率" prop="taxRate">
							<el-input-number v-model="state.ruleForm.taxRate" placeholder="请输入税率" clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="是否启用" prop="isEnabled">
							<el-input-number v-model="state.ruleForm.isEnabled" placeholder="请输入是否启用" clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="是否批次管理" prop="isBatchManage">
							<el-input-number v-model="state.ruleForm.isBatchManage" placeholder="请输入是否批次管理" clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="是否效期管理" prop="isExpiryManage">
							<el-input-number v-model="state.ruleForm.isExpiryManage" placeholder="请输入是否效期管理" clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
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