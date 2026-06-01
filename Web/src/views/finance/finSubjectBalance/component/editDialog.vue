<script lang="ts" name="finSubjectBalance" setup>
import { ref, reactive, onMounted } from "vue";
import { ElMessage } from "element-plus";
import type { FormRules } from "element-plus";
import { formatDate } from '/@/utils/formatTime';
import { useFinSubjectBalanceApi } from '/@/api/finance/finSubjectBalance';
//父级传递来的函数，用于回调
const emit = defineEmits(["reloadTable"]);
const finSubjectBalanceApi = useFinSubjectBalanceApi();
const ruleFormRef = ref();

const state = reactive({
	title: '',
	loading: false,
	showDialog: false,
	ruleForm: {} as any,
	stores: {},
	dropdownData: {} as any,
	accountSelector: [] as any[], // 科目下拉数据
});

// 自行添加其他规则
const rules = ref<FormRules>({
	accountId: [{required: true, message: '请选择科目！', trigger: 'change',}],
	year: [{required: true, message: '请选择会计年度！', trigger: 'blur',}],
	period: [{required: true, message: '请选择会计期间！', trigger: 'blur',}],
	initialDebit: [{required: true, message: '请输入期初借方余额！', trigger: 'blur',}],
	initialCredit: [{required: true, message: '请输入期初贷方余额！', trigger: 'blur',}],
	debitYTD: [{required: true, message: '请输入借方本年累计！', trigger: 'blur',}],
	creditYTD: [{required: true, message: '请输入贷方本年累计！', trigger: 'blur',}],
	debitPeriod: [{required: true, message: '请输入借方本期发生！', trigger: 'blur',}],
	creditPeriod: [{required: true, message: '请输入贷方本期发生！', trigger: 'blur',}],
	endDebit: [{required: true, message: '请输入期末借方余额！', trigger: 'blur',}],
	endCredit: [{required: true, message: '请输入期末贷方余额！', trigger: 'blur',}],
});

// 页面加载时
onMounted(async () => {
	await loadDropdownData();
});

// 加载下拉数据
const loadDropdownData = async () => {
	try {
		const res = await finSubjectBalanceApi.selectorAccount();
		state.accountSelector = res.data.result || [];
	} catch (error) {
		console.error('加载科目下拉数据失败:', error);
	}
};

// 科目选择变化时自动填充科目ID
const onAccountChange = (accountId: number) => {
	state.ruleForm.accountId = accountId;
	const account = state.accountSelector.find(item => item.id === accountId);
	if (account) {
		state.ruleForm.accountCode = account.accountCode;
	} else {
		state.ruleForm.accountCode = '';
	}
};

// 打开弹窗
const openDialog = async (row: any, title: string) => {
	state.title = title;
	row = row ?? {  };
	state.ruleForm = row.id ? await finSubjectBalanceApi.detail(row.id).then(res => res.data.result) : JSON.parse(JSON.stringify(row));
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
			await finSubjectBalanceApi[state.ruleForm.id ? 'update' : 'add'](values);
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
	<div class="finSubjectBalance-container">
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
						<el-form-item label="科目" prop="accountId">
							<el-select v-model="state.ruleForm.accountId" clearable filterable placeholder="请选择科目" style="width: 100%" @change="onAccountChange">
								<el-option v-for="item in state.accountSelector" :key="item.id" :label="`${item.accountCode} - ${item.accountName}`" :value="item.id" />
							</el-select>
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="科目编码" prop="accountCode">
							<el-input v-model="state.ruleForm.accountCode" placeholder="科目编码" maxlength="50" show-word-limit clearable disabled />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="会计年度" prop="year">
							<el-input-number v-model="state.ruleForm.year" placeholder="请输入会计年度" clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="会计期间" prop="period">
							<el-input-number v-model="state.ruleForm.period" placeholder="请输入会计期间" clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="期初借方余额" prop="initialDebit">
							<el-input-number v-model="state.ruleForm.initialDebit" placeholder="请输入期初借方余额" clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="期初贷方余额" prop="initialCredit">
							<el-input-number v-model="state.ruleForm.initialCredit" placeholder="请输入期初贷方余额" clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="借方本年累计" prop="debitYTD">
							<el-input-number v-model="state.ruleForm.debitYTD" placeholder="请输入借方本年累计" clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="贷方本年累计" prop="creditYTD">
							<el-input-number v-model="state.ruleForm.creditYTD" placeholder="请输入贷方本年累计" clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="借方本期发生" prop="debitPeriod">
							<el-input-number v-model="state.ruleForm.debitPeriod" placeholder="请输入借方本期发生" clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="贷方本期发生" prop="creditPeriod">
							<el-input-number v-model="state.ruleForm.creditPeriod" placeholder="请输入贷方本期发生" clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="期末借方余额" prop="endDebit">
							<el-input-number v-model="state.ruleForm.endDebit" placeholder="请输入期末借方余额" clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="期末贷方余额" prop="endCredit">
							<el-input-number v-model="state.ruleForm.endCredit" placeholder="请输入期末贷方余额" clearable />
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