<script lang="ts" name="finReceipt" setup>
import { ref, reactive, onMounted } from "vue";
import { ElMessage } from "element-plus";
import type { FormRules } from "element-plus";
import { formatDate } from '/@/utils/formatTime';
import { useFinReceiptApi } from '/@/api/finance/finReceipt';

const emit = defineEmits(["reloadTable"]);
const finReceiptApi = useFinReceiptApi();
const ruleFormRef = ref();

const state = reactive({
	title: '',
	loading: false,
	showDialog: false,
	ruleForm: {} as any,
	stores: {},
	dropdownData: {} as any,
	receiptTypeOptions: [] as any[],
	statusOptions: [] as any[],
	customerOptions: [] as any[],
	bankAccountOptions: [] as any[],
});

const rules = ref<FormRules>({
  customerId: [{required: true, message: '请选择客户！', trigger: 'change',},],
  customerName: [{required: true, message: '请选择客户名称！', trigger: 'change',},],
  receiptDate: [{required: true, message: '请选择收款日期！', trigger: 'change',},],
  receiptType: [{required: true, message: '请选择收款类型！', trigger: 'change',},],
  receiptAmount: [{required: true, message: '请选择收款金额！', trigger: 'blur',},],
  receivedAmount: [{required: true, message: '请选择已核销金额！', trigger: 'blur',},],
  unverifyAmount: [{required: true, message: '请选择未核销金额！', trigger: 'blur',},],
  status: [{required: true, message: '请选择状态！', trigger: 'change',},],
});

onMounted(async () => {
	await loadDropdownData();
});

const loadDropdownData = async () => {
	try {
		const receiptTypeRes = await finReceiptApi.getReceiptTypes();
		state.receiptTypeOptions = receiptTypeRes.data.result || [];
		
		const statusRes = await finReceiptApi.getStatuses();
		state.statusOptions = statusRes.data.result || [];
		
		const customerRes = await finReceiptApi.getCustomerSelector();
		state.customerOptions = customerRes.data.result || [];
		
		const bankAccountRes = await finReceiptApi.getBankAccountSelector();
		state.bankAccountOptions = bankAccountRes.data.result || [];
	} catch (error) {
		console.error('加载下拉数据失败:', error);
	}
};

const openDialog = async (row: any, title: string) => {
	state.title = title;
	row = row ?? {};
	if (!row.id) {
		try {
			const res = await finReceiptApi.getNewReceiptNo();
			row.receiptNo = res.data.result;
		} catch (error) {
			console.error('获取收款单号失败:', error);
		}
	}
	state.ruleForm = row.id ? await finReceiptApi.detail(row.id).then(res => res.data.result) : JSON.parse(JSON.stringify(row));
	state.showDialog = true;
};

const handleCustomerChange = (customerId: number) => {
	if (customerId) {
		const selectedCustomer = state.customerOptions.find(item => item.id === customerId);
		if (selectedCustomer) {
			state.ruleForm.customerId = selectedCustomer.id;
			state.ruleForm.customerName = selectedCustomer.customerName;
		}
	} else {
		state.ruleForm.customerId = null;
		state.ruleForm.customerName = '';
	}
};

const handleBankAccountChange = (bankAccountId: number) => {
	if (bankAccountId) {
		const selectedAccount = state.bankAccountOptions.find(item => item.id === bankAccountId);
		if (selectedAccount) {
			state.ruleForm.bankAccountId = selectedAccount.id;
			state.ruleForm.bankAccountName = selectedAccount.accountName;
		}
	} else {
		state.ruleForm.bankAccountId = null;
		state.ruleForm.bankAccountName = '';
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
			await finReceiptApi[state.ruleForm.id ? 'update' : 'add'](values);
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
	<div class="finReceipt-container">
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
						<el-form-item label="收款单号" prop="receiptNo">
							<el-input v-model="state.ruleForm.receiptNo" placeholder="系统自动生成" maxlength="50" show-word-limit clearable disabled />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="客户" prop="customerId">
							<el-select v-model="state.ruleForm.customerId" clearable filterable placeholder="请选择客户" style="width: 100%" @change="handleCustomerChange">
								<el-option v-for="item in state.customerOptions" :key="item.id" :label="`${item.customerCode} - ${item.customerName}`" :value="item.id" />
							</el-select>
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="客户名称" prop="customerName">
							<el-input v-model="state.ruleForm.customerName" placeholder="系统自动填充" maxlength="100" show-word-limit clearable disabled />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="收款日期" prop="receiptDate">
							<el-date-picker v-model="state.ruleForm.receiptDate" type="date" placeholder="收款日期" style="width: 100%" />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="收款类型" prop="receiptType">
							<el-select v-model="state.ruleForm.receiptType" clearable filterable placeholder="请选择收款类型" style="width: 100%">
								<el-option v-for="item in state.receiptTypeOptions" :key="item.code" :label="item.name" :value="item.code" />
							</el-select>
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="收款银行账户" prop="bankAccountId">
							<el-select v-model="state.ruleForm.bankAccountId" clearable filterable placeholder="请选择收款银行账户" style="width: 100%" @change="handleBankAccountChange">
								<el-option v-for="item in state.bankAccountOptions" :key="item.id" :label="`${item.accountCode} - ${item.accountName} (${item.bankName || ''})`" :value="item.id" />
							</el-select>
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="银行账户名称" prop="bankAccountName">
							<el-input v-model="state.ruleForm.bankAccountName" placeholder="系统自动填充" maxlength="100" show-word-limit clearable disabled />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="收款金额" prop="receiptAmount">
							<el-input-number v-model="state.ruleForm.receiptAmount" placeholder="请输入收款金额" clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="已核销金额" prop="receivedAmount">
							<el-input-number v-model="state.ruleForm.receivedAmount" placeholder="请输入已核销金额" clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="未核销金额" prop="unverifyAmount">
							<el-input-number v-model="state.ruleForm.unverifyAmount" placeholder="请输入未核销金额" clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="核销单号" prop="againstNo">
							<el-input v-model="state.ruleForm.againstNo" placeholder="请输入核销单号" maxlength="50" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="状态" prop="status">
							<el-select v-model="state.ruleForm.status" clearable filterable placeholder="请选择状态" style="width: 100%">
								<el-option v-for="item in state.statusOptions" :key="item.code" :label="item.name" :value="item.code" />
							</el-select>
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="审批人ID" prop="approvalUserId">
							<el-input v-model="state.ruleForm.approvalUserId" placeholder="请输入审批人ID" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="审批时间" prop="approvalTime">
							<el-date-picker v-model="state.ruleForm.approvalTime" type="date" placeholder="审批时间" />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="审批意见" prop="approverRemark">
							<el-input v-model="state.ruleForm.approverRemark" placeholder="请输入审批意见" maxlength="500" show-word-limit clearable />
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