<script lang="ts" name="finPayable" setup>
import { ref, reactive, onMounted } from "vue";
import { ElMessage } from "element-plus";
import type { FormRules } from "element-plus";
import { formatDate } from '/@/utils/formatTime';
import { useFinPayableApi } from '/@/api/finance/finPayable';

const emit = defineEmits(["reloadTable"]);
const finPayableApi = useFinPayableApi();
const ruleFormRef = ref();

const state = reactive({
	title: '',
	loading: false,
	showDialog: false,
	ruleForm: {} as any,
	stores: {},
	dropdownData: {} as any,
	statusOptions: [] as any[],
	supplierOptions: [] as any[],
});

const rules = ref<FormRules>({
  supplierId: [{required: true, message: '请选择供应商！', trigger: 'change',},],
  supplierCode: [{required: true, message: '请选择供应商编码！', trigger: 'change',},],
  supplierName: [{required: true, message: '请选择供应商名称！', trigger: 'change',},],
  billDate: [{required: true, message: '请选择单据日期！', trigger: 'change',},],
  amount: [{required: true, message: '请选择应付金额！', trigger: 'blur',},],
  paidAmount: [{required: true, message: '请选择已付金额！', trigger: 'blur',},],
  unpaidAmount: [{required: true, message: '请选择未付金额！', trigger: 'blur',},],
  overdueDays: [{required: true, message: '请选择逾期天数！', trigger: 'blur',},],
  status: [{required: true, message: '请选择状态！', trigger: 'change',},],
});

onMounted(async () => {
	await loadDropdownData();
});

const loadDropdownData = async () => {
	try {
		const statusRes = await finPayableApi.getStatuses();
		state.statusOptions = statusRes.data.result || [];
		
		const supplierRes = await finPayableApi.getSupplierSelector();
		state.supplierOptions = supplierRes.data.result || [];
	} catch (error) {
		console.error('加载下拉数据失败:', error);
	}
};

const openDialog = async (row: any, title: string) => {
	state.title = title;
	row = row ?? {};
	if (!row.id) {
		try {
			const res = await finPayableApi.getNewPayableNo();
			row.payableNo = res.data.result;
		} catch (error) {
			console.error('获取应付单号失败:', error);
		}
	}
	state.ruleForm = row.id ? await finPayableApi.detail(row.id).then(res => res.data.result) : JSON.parse(JSON.stringify(row));
	state.showDialog = true;
};

const handleSupplierChange = (supplierId: number) => {
	if (supplierId) {
		const selectedSupplier = state.supplierOptions.find(item => item.id === supplierId);
		if (selectedSupplier) {
			state.ruleForm.supplierId = selectedSupplier.id;
			state.ruleForm.supplierCode = selectedSupplier.supplierCode;
			state.ruleForm.supplierName = selectedSupplier.supplierName;
		}
	} else {
		state.ruleForm.supplierId = null;
		state.ruleForm.supplierCode = '';
		state.ruleForm.supplierName = '';
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
			await finPayableApi[state.ruleForm.id ? 'update' : 'add'](values);
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
	<div class="finPayable-container">
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
						<el-form-item label="应付单号" prop="payableNo">
							<el-input v-model="state.ruleForm.payableNo" placeholder="系统自动生成" maxlength="50" show-word-limit clearable disabled />
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
						<el-form-item label="供应商编码" prop="supplierCode">
							<el-input v-model="state.ruleForm.supplierCode" placeholder="系统自动填充" maxlength="50" show-word-limit clearable disabled />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="供应商名称" prop="supplierName">
							<el-input v-model="state.ruleForm.supplierName" placeholder="系统自动填充" maxlength="100" show-word-limit clearable disabled />
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
						<el-form-item label="应付金额" prop="amount">
							<el-input-number v-model="state.ruleForm.amount" placeholder="请输入应付金额" clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="已付金额" prop="paidAmount">
							<el-input-number v-model="state.ruleForm.paidAmount" placeholder="请输入已付金额" clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="未付金额" prop="unpaidAmount">
							<el-input-number v-model="state.ruleForm.unpaidAmount" placeholder="请输入未付金额" clearable />
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
						<el-form-item label="采购员ID" prop="purchaserId">
							<el-input v-model="state.ruleForm.purchaserId" placeholder="请输入采购员ID" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="采购员姓名" prop="purchaserName">
							<el-input v-model="state.ruleForm.purchaserName" placeholder="请输入采购员姓名" maxlength="50" show-word-limit clearable />
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