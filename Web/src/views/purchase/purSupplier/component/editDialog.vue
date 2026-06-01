<script lang="ts" name="purSupplier" setup>
import { ref, reactive, onMounted } from "vue";
import { ElMessage } from "element-plus";
import type { FormRules } from "element-plus";
import { formatDate } from '/@/utils/formatTime';
import { usePurSupplierApi } from '/@/api/purchase/purSupplier';
import { usePurSupplierCategoryApi } from '/@/api/purchase/purSupplierCategory';
//父级传递来的函数，用于回调
const emit = defineEmits(["reloadTable"]);
const purSupplierApi = usePurSupplierApi();
const purSupplierCategoryApi = usePurSupplierCategoryApi();
const ruleFormRef = ref();

const state = reactive({
	title: '',
	loading: false,
	showDialog: false,
	ruleForm: {} as any,
	stores: {},
	dropdownData: {} as any,
	categoryOptions: [] as any[],
	creditRatingOptions: [
		{ label: '1级', value: 1 },
		{ label: '2级', value: 2 },
		{ label: '3级', value: 3 },
		{ label: '4级', value: 4 },
		{ label: '5级', value: 5 },
	],
	statusOptions: [
		{ label: '禁用', value: 0 },
		{ label: '启用', value: 1 },
	],
});

// 自行添加其他规则
const rules = ref<FormRules>({
  supplierName: [{required: true, message: '请输入供应商名称！', trigger: 'blur',},],
  creditRating: [{required: true, message: '请选择信用等级！', trigger: 'blur',},],
  status: [{required: true, message: '请选择状态！', trigger: 'blur',},],
});

// 页面加载时
onMounted(async () => {
	await loadCategoryOptions();
});

// 加载供应商分类选项
const loadCategoryOptions = async () => {
	const result = await purSupplierCategoryApi.page({ page: 1, pageSize: 9999 }).then(res => res.data.result);
	state.categoryOptions = result.items.map((item: any) => ({
		label: item.categoryName,
		value: item.id,
	}));
};

// 打开弹窗
const openDialog = async (row: any, title: string) => {
	state.title = title;
	row = row ?? {  };
	state.ruleForm = row.id ? await purSupplierApi.detail(row.id).then(res => res.data.result) : JSON.parse(JSON.stringify(row));
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
			await purSupplierApi[state.ruleForm.id ? 'update' : 'add'](values);
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
	<div class="purSupplier-container">
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
						<el-form-item label="供应商编码">
							<el-input v-model="state.ruleForm.supplierCode" placeholder="系统自动生成" :disabled="!state.ruleForm.id" maxlength="50" show-word-limit />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="供应商名称" prop="supplierName">
							<el-input v-model="state.ruleForm.supplierName" placeholder="请输入供应商名称" maxlength="100" show-word-limit clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="简称" prop="shortName">
							<el-input v-model="state.ruleForm.shortName" placeholder="请输入简称" maxlength="50" show-word-limit clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="供应商分类" prop="categoryId">
							<el-select v-model="state.ruleForm.categoryId" placeholder="请选择供应商分类" clearable filterable>
								<el-option v-for="item in state.categoryOptions" :key="item.value" :label="item.label" :value="item.value" />
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="联系人" prop="contactName">
							<el-input v-model="state.ruleForm.contactName" placeholder="请输入联系人" maxlength="50" show-word-limit clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="联系电话" prop="phone">
							<el-input v-model="state.ruleForm.phone" placeholder="请输入联系电话" maxlength="20" show-word-limit clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="手机" prop="mobile">
							<el-input v-model="state.ruleForm.mobile" placeholder="请输入手机" maxlength="20" show-word-limit clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="邮箱" prop="email">
							<el-input v-model="state.ruleForm.email" placeholder="请输入邮箱" maxlength="100" show-word-limit clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="地址" prop="address">
							<el-input v-model="state.ruleForm.address" placeholder="请输入地址" maxlength="500" show-word-limit clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="开户银行" prop="bankName">
							<el-input v-model="state.ruleForm.bankName" placeholder="请输入开户银行" maxlength="100" show-word-limit clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="银行账号" prop="bankAccount">
							<el-input v-model="state.ruleForm.bankAccount" placeholder="请输入银行账号" maxlength="50" show-word-limit clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="税号" prop="taxNo">
							<el-input v-model="state.ruleForm.taxNo" placeholder="请输入税号" maxlength="50" show-word-limit clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="信用等级" prop="creditRating">
							<el-select v-model="state.ruleForm.creditRating" placeholder="请选择信用等级">
								<el-option v-for="item in state.creditRatingOptions" :key="item.value" :label="item.label" :value="item.value" />
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="状态" prop="status">
							<el-select v-model="state.ruleForm.status" placeholder="请选择状态">
								<el-option v-for="item in state.statusOptions" :key="item.value" :label="item.label" :value="item.value" />
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="是否黑名单" prop="isBlacklist">
							<el-switch v-model="state.ruleForm.isBlacklist" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="备注" prop="remark">
							<el-input v-model="state.ruleForm.remark" placeholder="请输入备注" maxlength="500" show-word-limit clearable />
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