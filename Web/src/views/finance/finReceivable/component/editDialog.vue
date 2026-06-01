<script lang="ts" name="finReceivable" setup>
import { ref, reactive, onMounted } from "vue";
import { ElMessage } from "element-plus";
import type { FormRules } from "element-plus";
import { formatDate } from '/@/utils/formatTime';
import { useFinReceivableApi } from '/@/api/finance/finReceivable';

const emit = defineEmits(["reloadTable"]);
const finReceivableApi = useFinReceivableApi();
const ruleFormRef = ref();

const state = reactive({
	title: '',
	loading: false,
	showDialog: false,
	ruleForm: {} as any,
	stores: {},
	dropdownData: {} as any,
	statusOptions: [] as any[],
	customerOptions: [] as any[],
});

const rules = ref<FormRules>({
  customerId: [{required: true, message: '请选择客户！', trigger: 'change',},],
  customerCode: [{required: true, message: '请选择客户编码！', trigger: 'change',},],
  customerName: [{required: true, message: '请选择客户名称！', trigger: 'change',},],
  billDate: [{required: true, message: '请选择单据日期！', trigger: 'change',},],
  amount: [{required: true, message: '请选择应收金额！', trigger: 'blur',},],
  receivedAmount: [{required: true, message: '请选择已收金额！', trigger: 'blur',},],
  unreceivedAmount: [{required: true, message: '请选择未收金额！', trigger: 'blur',},],
  overdueDays: [{required: true, message: '请选择逾期天数！', trigger: 'blur',},],
  status: [{required: true, message: '请选择状态！', trigger: 'change',},],
});

onMounted(async () => {
	await loadDropdownData();
});

const loadDropdownData = async () => {
	try {
		const statusRes = await finReceivableApi.getStatuses();
		state.statusOptions = statusRes.data.result || [];
		
		const customerRes = await finReceivableApi.getCustomerSelector();
		state.customerOptions = customerRes.data.result || [];
	} catch (error) {
		console.error('加载下拉数据失败:', error);
	}
};

const openDialog = async (row: any, title: string) => {
	state.title = title;
	row = row ?? {};
	if (!row.id) {
		try {
			const res = await finReceivableApi.getNewReceivableNo();
			row.receivableNo = res.data.result;
		} catch (error) {
			console.error('获取应收单号失败:', error);
		}
	}
	state.ruleForm = row.id ? await finReceivableApi.detail(row.id).then(res => res.data.result) : JSON.parse(JSON.stringify(row));
	state.showDialog = true;
};

const handleCustomerChange = (customerId: number) => {
	if (customerId) {
		const selectedCustomer = state.customerOptions.find(item => item.id === customerId);
		if (selectedCustomer) {
			state.ruleForm.customerId = selectedCustomer.id;
			state.ruleForm.customerCode = selectedCustomer.customerCode;
			state.ruleForm.customerName = selectedCustomer.customerName;
		}
	} else {
		state.ruleForm.customerId = null;
		state.ruleForm.customerCode = '';
		state.ruleForm.customerName = '';
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
			await finReceivableApi[state.ruleForm.id ? 'update' : 'add'](values);
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
	<div class="finReceivable-container">
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
						<el-form-item label="应收单号" prop="receivableNo">
							<el-input v-model="state.ruleForm.receivableNo" placeholder="系统自动生成" maxlength="50" show-word-limit clearable disabled />
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
						<el-form-item label="客户编码" prop="customerCode">
							<el-input v-model="state.ruleForm.customerCode" placeholder="系统自动填充" maxlength="50" show-word-limit clearable disabled />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="客户名称" prop="customerName">
							<el-input v-model="state.ruleForm.customerName" placeholder="系统自动填充" maxlength="100" show-word-limit clearable disabled />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="来源单据类型" prop="sourceType">
							<el-input v-model="state.ruleForm.sourceType" placeholder="请输入来源单据类型" maxlength="50" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="来源单据ID" prop="sourceId">
							<el-input v-model="state.ruleForm.sourceId" placeholder="请输入来源单据ID" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="来源单据号" prop="sourceNo">
							<el-input v-model="state.ruleForm.sourceNo" placeholder="请输入来源单据号" maxlength="50" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="单据日期" prop="billDate">
							<el-date-picker v-model="state.ruleForm.billDate" type="date" placeholder="单据日期" />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="到期日期" prop="dueDate">
							<el-date-picker v-model="state.ruleForm.dueDate" type="date" placeholder="到期日期" />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="应收金额" prop="amount">
							<el-input-number v-model="state.ruleForm.amount" placeholder="请输入应收金额" clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="已收金额" prop="receivedAmount">
							<el-input-number v-model="state.ruleForm.receivedAmount" placeholder="请输入已收金额" clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="未收金额" prop="unreceivedAmount">
							<el-input-number v-model="state.ruleForm.unreceivedAmount" placeholder="请输入未收金额" clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="逾期天数" prop="overdueDays">
							<el-input-number v-model="state.ruleForm.overdueDays" placeholder="请输入逾期天数" clearable />
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
						<el-form-item label="业务员ID" prop="salesmanId">
							<el-input v-model="state.ruleForm.salesmanId" placeholder="请输入业务员ID" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="业务员姓名" prop="salesmanName">
							<el-input v-model="state.ruleForm.salesmanName" placeholder="请输入业务员姓名" maxlength="50" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="部门ID" prop="departmentId">
							<el-input v-model="state.ruleForm.departmentId" placeholder="请输入部门ID" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="部门名称" prop="departmentName">
							<el-input v-model="state.ruleForm.departmentName" placeholder="请输入部门名称" maxlength="100" show-word-limit clearable />
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