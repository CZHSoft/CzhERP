<script lang="ts" name="finVoucherDetail" setup>
import { ref, reactive, onMounted } from "vue";
import { ElMessage } from "element-plus";
import type { FormRules } from "element-plus";
import { useFinVoucherDetailApi } from '/@/api/finance/finVoucherDetail';
//父级传递来的函数，用于回调
const emit = defineEmits(["reloadTable"]);
const finVoucherDetailApi = useFinVoucherDetailApi();
const ruleFormRef = ref();

const state = reactive({
	title: '',
	loading: false,
	showDialog: false,
	ruleForm: {} as any,
	stores: {},
	dropdownData: {} as any,
	voucherSelector: [] as any[], // 凭证下拉数据
	accountSelector: [] as any[], // 科目下拉数据
});

// 自行添加其他规则
const rules = ref<FormRules>({
	voucherId: [{required: true, message: '请选择凭证ID！', trigger: 'change',}],
	accountId: [{required: true, message: '请选择科目！', trigger: 'change',}],
	debitAmount: [{required: true, message: '请选择借方金额！', trigger: 'blur',}],
	creditAmount: [{required: true, message: '请选择贷方金额！', trigger: 'blur',}],
	sortOrder: [{required: true, message: '请选择排序号！', trigger: 'blur',}],
});

// 页面加载时
onMounted(async () => {
	await loadDropdownData();
});

// 加载下拉数据
const loadDropdownData = async () => {
	try {
		const [voucherRes, accountRes] = await Promise.all([
			finVoucherDetailApi.selectorVoucher(),
			finVoucherDetailApi.selectorAccount()
		]);
		state.voucherSelector = voucherRes.data.result || [];
		state.accountSelector = accountRes.data.result || [];
	} catch (error) {
		console.error('加载下拉数据失败:', error);
	}
};

// 科目选择变化时自动填充科目编码和名称
const onAccountChange = (accountId: number) => {
	const account = state.accountSelector.find(item => item.id === accountId);
	if (account) {
		state.ruleForm.accountCode = account.accountCode;
		state.ruleForm.accountName = account.accountName;
	}
};

// 打开弹窗
const openDialog = async (row: any, title: string) => {
	state.title = title;
	row = row ?? {  };
	state.ruleForm = row.id ? await finVoucherDetailApi.detail(row.id).then(res => res.data.result) : JSON.parse(JSON.stringify(row));
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
			await finVoucherDetailApi[state.ruleForm.id ? 'update' : 'add'](values);
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
	<div class="finVoucherDetail-container">
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
						<el-form-item label="凭证" prop="voucherId">
							<el-select v-model="state.ruleForm.voucherId" clearable filterable placeholder="请选择凭证" style="width: 100%">
								<el-option v-for="item in state.voucherSelector" :key="item.id" :label="`${item.voucherNo} - ${item.remark || ''}`" :value="item.id" />
							</el-select>
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="科目" prop="accountId">
							<el-select v-model="state.ruleForm.accountId" clearable filterable placeholder="请选择科目" style="width: 100%" @change="onAccountChange">
								<el-option v-for="item in state.accountSelector" :key="item.id" :label="`${item.accountCode} - ${item.accountName}`" :value="item.id" />
							</el-select>
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="科目编码" prop="accountCode">
							<el-input v-model="state.ruleForm.accountCode" placeholder="请输入科目编码" maxlength="50" show-word-limit clearable disabled />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="科目名称" prop="accountName">
							<el-input v-model="state.ruleForm.accountName" placeholder="请输入科目名称" maxlength="100" show-word-limit clearable disabled />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="摘要" prop="summary">
							<el-input v-model="state.ruleForm.summary" placeholder="请输入摘要" maxlength="200" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="借方金额" prop="debitAmount">
							<el-input-number v-model="state.ruleForm.debitAmount" placeholder="请输入借方金额" clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="贷方金额" prop="creditAmount">
							<el-input-number v-model="state.ruleForm.creditAmount" placeholder="请输入贷方金额" clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="部门ID" prop="deptId">
							<el-input v-model="state.ruleForm.deptId" placeholder="请输入部门ID" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="部门名称" prop="deptName">
							<el-input v-model="state.ruleForm.deptName" placeholder="请输入部门名称" maxlength="100" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="个人ID" prop="personId">
							<el-input v-model="state.ruleForm.personId" placeholder="请输入个人ID" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="个人姓名" prop="personName">
							<el-input v-model="state.ruleForm.personName" placeholder="请输入个人姓名" maxlength="50" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="供应商ID" prop="supplierId">
							<el-input v-model="state.ruleForm.supplierId" placeholder="请输入供应商ID" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="供应商名称" prop="supplierName">
							<el-input v-model="state.ruleForm.supplierName" placeholder="请输入供应商名称" maxlength="100" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="客户ID" prop="customerId">
							<el-input v-model="state.ruleForm.customerId" placeholder="请输入客户ID" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="客户名称" prop="customerName">
							<el-input v-model="state.ruleForm.customerName" placeholder="请输入客户名称" maxlength="100" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="项目ID" prop="projectId">
							<el-input v-model="state.ruleForm.projectId" placeholder="请输入项目ID" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="项目名称" prop="projectName">
							<el-input v-model="state.ruleForm.projectName" placeholder="请输入项目名称" maxlength="100" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="存货ID" prop="inventoryId">
							<el-input v-model="state.ruleForm.inventoryId" placeholder="请输入存货ID" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="存货名称" prop="inventoryName">
							<el-input v-model="state.ruleForm.inventoryName" placeholder="请输入存货名称" maxlength="100" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="现金流量编码" prop="cashFlowCode">
							<el-input v-model="state.ruleForm.cashFlowCode" placeholder="请输入现金流量编码" maxlength="50" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="现金流量名称" prop="cashFlowName">
							<el-input v-model="state.ruleForm.cashFlowName" placeholder="请输入现金流量名称" maxlength="100" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="排序号" prop="sortOrder">
							<el-input-number v-model="state.ruleForm.sortOrder" placeholder="请输入排序号" clearable />
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