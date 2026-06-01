<script lang="ts" name="finBankStatement" setup>
import { ref, reactive, onMounted } from "vue";
import { ElMessage } from "element-plus";
import type { FormRules } from "element-plus";
import { formatDate } from '/@/utils/formatTime';
import { useFinBankStatementApi } from '/@/api/finance/finBankStatement';

const emit = defineEmits(["reloadTable"]);
const finBankStatementApi = useFinBankStatementApi();
const ruleFormRef = ref();

const state = reactive({
	title: '',
	loading: false,
	showDialog: false,
	ruleForm: {} as any,
	stores: {},
	dropdownData: {} as any,
	bankAccountOptions: [] as any[],
});

const rules = ref<FormRules>({
  bankAccountId: [{required: true, message: '请选择银行账户！', trigger: 'change',},],
  statementDate: [{required: true, message: '请选择对账日期！', trigger: 'change',},],
  transactionDate: [{required: true, message: '请选择交易日期！', trigger: 'change',},],
  transactionType: [{required: true, message: '请选择交易类型！', trigger: 'blur',},],
  amount: [{required: true, message: '请选择交易金额！', trigger: 'blur',},],
  balance: [{required: true, message: '请选择余额！', trigger: 'blur',},],
});

onMounted(async () => {
	await loadDropdownData();
});

const loadDropdownData = async () => {
	try {
		const bankAccountRes = await finBankStatementApi.getBankAccountSelector();
		state.bankAccountOptions = bankAccountRes.data.result || [];
	} catch (error) {
		console.error('加载下拉数据失败:', error);
	}
};

const openDialog = async (row: any, title: string) => {
	state.title = title;
	row = row ?? {};
	state.ruleForm = row.id ? await finBankStatementApi.detail(row.id).then(res => res.data.result) : JSON.parse(JSON.stringify(row));
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
			await finBankStatementApi[state.ruleForm.id ? 'update' : 'add'](values);
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
	<div class="finBankStatement-container">
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
						<el-form-item label="银行账户" prop="bankAccountId">
							<el-select v-model="state.ruleForm.bankAccountId" clearable filterable placeholder="请选择银行账户" style="width: 100%">
								<el-option v-for="item in state.bankAccountOptions" :key="item.id" :label="`${item.accountCode} - ${item.accountName} (${item.bankAccount || ''})`" :value="item.id" />
							</el-select>
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="银行账号" prop="bankAccountNo">
							<el-input v-model="state.ruleForm.bankAccountNo" placeholder="请输入银行账号" maxlength="50" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="对账日期" prop="statementDate">
							<el-date-picker v-model="state.ruleForm.statementDate" type="date" placeholder="对账日期" style="width: 100%" />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="交易日期" prop="transactionDate">
							<el-date-picker v-model="state.ruleForm.transactionDate" type="date" placeholder="交易日期" style="width: 100%" />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="交易类型" prop="transactionType">
							<el-input v-model="state.ruleForm.transactionType" placeholder="请输入交易类型" maxlength="20" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="交易金额" prop="amount">
							<el-input-number v-model="state.ruleForm.amount" placeholder="请输入交易金额" clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="余额" prop="balance">
							<el-input-number v-model="state.ruleForm.balance" placeholder="请输入余额" clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="对方单位" prop="counterparty">
							<el-input v-model="state.ruleForm.counterparty" placeholder="请输入对方单位" maxlength="100" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="交易描述" prop="description">
							<el-input v-model="state.ruleForm.description" placeholder="请输入交易描述" maxlength="500" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="是否已匹配" prop="isMatched">
							<el-switch v-model="state.ruleForm.isMatched" />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="匹配凭证ID" prop="matchedVoucherId">
							<el-input v-model="state.ruleForm.matchedVoucherId" placeholder="请输入匹配凭证ID" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="是否已对账" prop="isReconciled">
							<el-switch v-model="state.ruleForm.isReconciled" />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="对账人ID" prop="reconcileUserId">
							<el-input v-model="state.ruleForm.reconcileUserId" placeholder="请输入对账人ID" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="对账时间" prop="reconcileTime">
							<el-date-picker v-model="state.ruleForm.reconcileTime" type="date" placeholder="对账时间" style="width: 100%" />
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