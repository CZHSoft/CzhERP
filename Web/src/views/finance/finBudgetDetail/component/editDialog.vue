<script lang="ts" name="finBudgetDetail" setup>
import { ref, reactive, onMounted } from "vue";
import { ElMessage } from "element-plus";
import type { FormRules } from "element-plus";
import { useFinBudgetDetailApi } from '/@/api/finance/finBudgetDetail';

const emit = defineEmits(["reloadTable"]);
const finBudgetDetailApi = useFinBudgetDetailApi();
const ruleFormRef = ref();

const state = reactive({
	title: '',
	loading: false,
	showDialog: false,
	ruleForm: {} as any,
	stores: {},
	dropdownData: {} as any,
	budgetOptions: [] as any[],
	accountOptions: [] as any[],
});

const rules = ref<FormRules>({
  budgetId: [{required: true, message: '请选择预算！', trigger: 'change',},],
  period: [{required: true, message: '请选择预算期间！', trigger: 'blur',},],
  budgetAmount: [{required: true, message: '请选择预算金额！', trigger: 'blur',},],
  executedAmount: [{required: true, message: '请选择已执行金额！', trigger: 'blur',},],
  remainAmount: [{required: true, message: '请选择剩余金额！', trigger: 'blur',},],
});

onMounted(async () => {
	await loadDropdownData();
});

const loadDropdownData = async () => {
	try {
		const budgetRes = await finBudgetDetailApi.getBudgetSelector();
		state.budgetOptions = budgetRes.data.result || [];
		
		const accountRes = await finBudgetDetailApi.getAccountSelector();
		state.accountOptions = accountRes.data.result || [];
	} catch (error) {
		console.error('加载下拉数据失败:', error);
	}
};

const openDialog = async (row: any, title: string) => {
	state.title = title;
	row = row ?? {};
	state.ruleForm = row.id ? await finBudgetDetailApi.detail(row.id).then(res => res.data.result) : JSON.parse(JSON.stringify(row));
	state.showDialog = true;
};

const handleAccountChange = (accountId: number) => {
	if (accountId) {
		const selectedAccount = state.accountOptions.find(item => item.id === accountId);
		if (selectedAccount) {
			state.ruleForm.accountCode = selectedAccount.accountCode;
			state.ruleForm.accountName = selectedAccount.accountName;
		}
	} else {
		state.ruleForm.accountCode = '';
		state.ruleForm.accountName = '';
	}
};

const closeDialog = () => {
	emit("reloadTable");
	state.showDialog = false;
};

const submit = async () => {
	ruleFormRef.value.validate(async (isValid: boolean, fields?: any) => {
		if (isValid) {
			let values = state.ruleForm;
			await finBudgetDetailApi[state.ruleForm.id ? 'update' : 'add'](values);
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
	<div class="finBudgetDetail-container">
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
						<el-form-item label="预算" prop="budgetId">
							<el-select v-model="state.ruleForm.budgetId" clearable filterable placeholder="请选择预算" style="width: 100%">
								<el-option v-for="item in state.budgetOptions" :key="item.id" :label="`${item.budgetNo} - ${item.budgetName}`" :value="item.id" />
							</el-select>
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="预算期间" prop="period">
							<el-input-number v-model="state.ruleForm.period" placeholder="请输入预算期间" clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="科目" prop="accountId">
							<el-select v-model="state.ruleForm.accountId" clearable filterable placeholder="请选择科目" style="width: 100%" @change="handleAccountChange">
								<el-option v-for="item in state.accountOptions" :key="item.id" :label="`${item.accountCode} - ${item.accountName}`" :value="item.id" />
							</el-select>
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="科目编码" prop="accountCode">
							<el-input v-model="state.ruleForm.accountCode" placeholder="系统自动填充" maxlength="50" show-word-limit clearable disabled />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="科目名称" prop="accountName">
							<el-input v-model="state.ruleForm.accountName" placeholder="系统自动填充" maxlength="100" show-word-limit clearable disabled />
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
						<el-form-item label="预算金额" prop="budgetAmount">
							<el-input-number v-model="state.ruleForm.budgetAmount" placeholder="请输入预算金额" clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="已执行金额" prop="executedAmount">
							<el-input-number v-model="state.ruleForm.executedAmount" placeholder="请输入已执行金额" clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="剩余金额" prop="remainAmount">
							<el-input-number v-model="state.ruleForm.remainAmount" placeholder="请输入剩余金额" clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="预警阈值" prop="warnThreshold">
							<el-input-number v-model="state.ruleForm.warnThreshold" placeholder="请输入预警阈值" clearable />
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