<script lang="ts" name="finCashAccount" setup>
import { ref, reactive, onMounted } from "vue";
import { ElMessage } from "element-plus";
import type { FormRules } from "element-plus";
import { formatDate } from '/@/utils/formatTime';
import { useFinCashAccountApi } from '/@/api/finance/finCashAccount';

const emit = defineEmits(["reloadTable"]);
const finCashAccountApi = useFinCashAccountApi();
const ruleFormRef = ref();

const state = reactive({
	title: '',
	loading: false,
	showDialog: false,
	ruleForm: {} as any,
	stores: {},
	dropdownData: {} as any,
	accountTypeOptions: [] as any[],
	currencyOptions: [] as any[],
});

const rules = ref<FormRules>({
  accountCode: [{required: true, message: '请选择账户编码！', trigger: 'blur',},],
  accountName: [{required: true, message: '请选择账户名称！', trigger: 'blur',},],
  accountType: [{required: true, message: '请选择账户类型！', trigger: 'change',},],
  openingBalance: [{required: true, message: '请选择期初余额！', trigger: 'blur',},],
  currentBalance: [{required: true, message: '请选择当前余额！', trigger: 'blur',},],
  currency: [{required: true, message: '请选择币种！', trigger: 'change',},],
  isEnabled: [{required: true, message: '请选择是否启用！', trigger: 'change',},],
});

onMounted(async () => {
	await loadDropdownData();
});

const loadDropdownData = async () => {
	try {
		const accountTypeRes = await finCashAccountApi.getAccountTypes();
		state.accountTypeOptions = accountTypeRes.data.result || [];
		
		const currencyRes = await finCashAccountApi.getCurrencies();
		state.currencyOptions = currencyRes.data.result || [];
	} catch (error) {
		console.error('加载下拉数据失败:', error);
	}
};

const openDialog = async (row: any, title: string) => {
	state.title = title;
	row = row ?? {};
	if (!row.id) {
		try {
			const res = await finCashAccountApi.getNewAccountCode();
			row.accountCode = res.data.result;
		} catch (error) {
			console.error('获取账户编码失败:', error);
		}
	}
	state.ruleForm = row.id ? await finCashAccountApi.detail(row.id).then(res => res.data.result) : JSON.parse(JSON.stringify(row));
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
			await finCashAccountApi[state.ruleForm.id ? 'update' : 'add'](values);
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
	<div class="finCashAccount-container">
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
						<el-form-item label="账户编码" prop="accountCode">
							<el-input v-model="state.ruleForm.accountCode" placeholder="系统自动生成" maxlength="50" show-word-limit clearable disabled />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="账户名称" prop="accountName">
							<el-input v-model="state.ruleForm.accountName" placeholder="请输入账户名称" maxlength="100" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="账户类型" prop="accountType">
							<el-select v-model="state.ruleForm.accountType" clearable filterable placeholder="请选择账户类型" style="width: 100%">
								<el-option v-for="item in state.accountTypeOptions" :key="item.code" :label="item.name" :value="item.code" />
							</el-select>
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
						<el-form-item label="期初余额" prop="openingBalance">
							<el-input-number v-model="state.ruleForm.openingBalance" placeholder="请输入期初余额" clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="当前余额" prop="currentBalance">
							<el-input-number v-model="state.ruleForm.currentBalance" placeholder="请输入当前余额" clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="币种" prop="currency">
							<el-select v-model="state.ruleForm.currency" clearable filterable placeholder="请选择币种" style="width: 100%">
								<el-option v-for="item in state.currencyOptions" :key="item.code" :label="item.name" :value="item.code" />
							</el-select>
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="是否启用" prop="isEnabled">
							<el-switch v-model="state.ruleForm.isEnabled" />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="是否默认账户" prop="isDefault">
							<el-switch v-model="state.ruleForm.isDefault" />
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