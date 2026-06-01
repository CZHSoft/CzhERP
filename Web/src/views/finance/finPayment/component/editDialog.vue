<script lang="ts" name="finPayment" setup>
import { ref, reactive, onMounted } from "vue";
import { ElMessage } from "element-plus";
import type { FormRules } from "element-plus";
import { formatDate } from '/@/utils/formatTime';
import { useFinPaymentApi } from '/@/api/finance/finPayment';

const emit = defineEmits(["reloadTable"]);
const finPaymentApi = useFinPaymentApi();
const ruleFormRef = ref();

const state = reactive({
	title: '',
	loading: false,
	showDialog: false,
	ruleForm: {} as any,
	stores: {},
	dropdownData: {} as any,
	paymentTypeOptions: [] as any[],
	paymentMethodOptions: [] as any[],
	statusOptions: [] as any[],
	supplierOptions: [] as any[],
	bankAccountOptions: [] as any[],
});

const rules = ref<FormRules>({
  supplierId: [{required: true, message: '请选择供应商！', trigger: 'change',},],
  supplierName: [{required: true, message: '请选择供应商名称！', trigger: 'change',},],
  paymentDate: [{required: true, message: '请选择付款日期！', trigger: 'change',},],
  paymentType: [{required: true, message: '请选择付款类型！', trigger: 'change',},],
  paymentAmount: [{required: true, message: '请选择付款金额！', trigger: 'blur',},],
  paidAmount: [{required: true, message: '请选择已核销金额！', trigger: 'blur',},],
  unverifyAmount: [{required: true, message: '请选择未核销金额！', trigger: 'blur',},],
  status: [{required: true, message: '请选择状态！', trigger: 'change',},],
});

onMounted(async () => {
	await loadDropdownData();
});

const loadDropdownData = async () => {
	try {
		const paymentTypeRes = await finPaymentApi.getPaymentTypes();
		state.paymentTypeOptions = paymentTypeRes.data.result || [];
		
		const paymentMethodRes = await finPaymentApi.getPaymentMethods();
		state.paymentMethodOptions = paymentMethodRes.data.result || [];
		
		const statusRes = await finPaymentApi.getStatuses();
		state.statusOptions = statusRes.data.result || [];
		
		const supplierRes = await finPaymentApi.getSupplierSelector();
		state.supplierOptions = supplierRes.data.result || [];
		
		const bankAccountRes = await finPaymentApi.getBankAccountSelector();
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
			const res = await finPaymentApi.getNewPaymentNo();
			row.paymentNo = res.data.result;
		} catch (error) {
			console.error('获取付款单号失败:', error);
		}
	}
	state.ruleForm = row.id ? await finPaymentApi.detail(row.id).then(res => res.data.result) : JSON.parse(JSON.stringify(row));
	state.showDialog = true;
};

const handleSupplierChange = (supplierId: number) => {
	if (supplierId) {
		const selectedSupplier = state.supplierOptions.find(item => item.id === supplierId);
		if (selectedSupplier) {
			state.ruleForm.supplierId = selectedSupplier.id;
			state.ruleForm.supplierName = selectedSupplier.supplierName;
		}
	} else {
		state.ruleForm.supplierId = null;
		state.ruleForm.supplierName = '';
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
			await finPaymentApi[state.ruleForm.id ? 'update' : 'add'](values);
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
	<div class="finPayment-container">
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
						<el-form-item label="付款单号" prop="paymentNo">
							<el-input v-model="state.ruleForm.paymentNo" placeholder="系统自动生成" maxlength="50" show-word-limit clearable disabled />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="供应商" prop="supplierId">
							<el-select v-model="state.ruleForm.supplierId" clearable filterable placeholder="请选择供应商" style="width: 100%" @change="handleSupplierChange">
								<el-option v-for="item in state.supplierOptions" :key="item.id" :label="`${item.supplierCode} - ${item.supplierName}`" :value="item.id" />
							</el-select>
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="供应商名称" prop="supplierName">
							<el-input v-model="state.ruleForm.supplierName" placeholder="系统自动填充" maxlength="100" show-word-limit clearable disabled />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="付款日期" prop="paymentDate">
							<el-date-picker v-model="state.ruleForm.paymentDate" type="date" placeholder="付款日期" style="width: 100%" />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="付款类型" prop="paymentType">
							<el-select v-model="state.ruleForm.paymentType" clearable filterable placeholder="请选择付款类型" style="width: 100%">
								<el-option v-for="item in state.paymentTypeOptions" :key="item.code" :label="item.name" :value="item.code" />
							</el-select>
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="付款银行账户" prop="bankAccountId">
							<el-select v-model="state.ruleForm.bankAccountId" clearable filterable placeholder="请选择付款银行账户" style="width: 100%" @change="handleBankAccountChange">
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
						<el-form-item label="付款金额" prop="paymentAmount">
							<el-input-number v-model="state.ruleForm.paymentAmount" placeholder="请输入付款金额" clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="已核销金额" prop="paidAmount">
							<el-input-number v-model="state.ruleForm.paidAmount" placeholder="请输入已核销金额" clearable />
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
						<el-form-item label="付款方式" prop="paymentMethod">
							<el-select v-model="state.ruleForm.paymentMethod" clearable filterable placeholder="请选择付款方式" style="width: 100%">
								<el-option v-for="item in state.paymentMethodOptions" :key="item.code" :label="item.name" :value="item.code" />
							</el-select>
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