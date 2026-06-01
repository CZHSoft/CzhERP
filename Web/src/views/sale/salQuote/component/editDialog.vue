<script lang="ts" name="salQuote" setup>
import { ref, reactive, onMounted } from "vue";
import { ElMessage } from "element-plus";
import type { FormRules } from "element-plus";
import { formatDate } from '/@/utils/formatTime';
import { useSalQuoteApi } from '/@/api/sale/salQuote';
//父级传递来的函数，用于回调
const emit = defineEmits(["reloadTable"]);
const salQuoteApi = useSalQuoteApi();
const ruleFormRef = ref();

const state = reactive({
	title: '',
	loading: false,
	showDialog: false,
	ruleForm: {} as any,
	stores: {},
	dropdownData: {} as any,
});

const customerOptions = ref<Array<{value: number; label: string; customerCode: string; contactName: string; contactPhone: string}>>([]);
const customerLoading = ref(false);
const customerSearchText = ref('');
const statusOptions = ref<Array<{value: string; label: string; description: string}>>([]);

// 自行添加其他规则
const rules = ref<FormRules>({
  quoteNo: [],
  customerId: [{required: true, message: '请选择客户！', trigger: 'change',},],
  customerName: [],
  quoteDate: [{required: true, message: '请选择报价日期！', trigger: 'change',},],
  validStartDate: [{required: true, message: '请选择有效开始日期！', trigger: 'change',},],
  validEndDate: [{required: true, message: '请选择有效结束日期！', trigger: 'change',},],
  totalAmount: [{required: true, message: '请选择总金额！', trigger: 'blur',},],
  totalTaxAmount: [{required: true, message: '请选择总税额！', trigger: 'blur',},],
  status: [{required: true, message: '请选择状态！', trigger: 'change',},],
});

const isEdit = ref(false);

// 获取客户列表
const loadCustomerList = async () => {
	if (customerLoading.value) return;
	
	customerLoading.value = true;
	try {
		const res = await salQuoteApi.getCustomerList({ keyword: customerSearchText.value });
		if (res && res.data && res.data.result) {
			customerOptions.value = res.data.result.map((item: any) => ({
				value: item.id,
				label: item.customerName,
				customerCode: item.customerCode,
				contactName: item.contactName,
				contactPhone: item.contactPhone,
			}));
		} else {
			customerOptions.value = [];
		}
	} catch (error) {
		console.error('获取客户列表失败:', error);
		customerOptions.value = [];
		ElMessage({
			message: '获取客户列表失败，请稍后重试',
			type: "error",
		});
	} finally {
		customerLoading.value = false;
	}
};

// 客户选择变化处理
const handleCustomerChange = (customerId: number) => {
	if (!customerId) {
		state.ruleForm.customerName = '';
		state.ruleForm.customerId = '';
		return;
	}
	
	const selectedCustomer = customerOptions.value.find(item => item.value === customerId);
	if (selectedCustomer) {
		state.ruleForm.customerName = selectedCustomer.label;
		state.ruleForm.customerId = selectedCustomer.value;
	}
};

// 搜索客户
const handleCustomerSearch = (keyword: string) => {
	customerSearchText.value = keyword;
};

// 下拉框打开时加载数据
const handleDropdownOpen = () => {
	if (customerOptions.value.length === 0) {
		loadCustomerList();
	}
};

// 获取状态选项
const loadStatusOptions = async () => {
	try {
		const res = await salQuoteApi.getStatusOptions();
		if (res && res.data && res.data.result) {
			statusOptions.value = res.data.result;
		}
	} catch (error) {
		console.error('获取状态选项失败:', error);
		statusOptions.value = [
			{ value: 'Draft', label: '草稿', description: '报价单处于草稿状态' },
			{ value: 'Approved', label: '已审批', description: '报价单已通过审批' },
			{ value: 'Rejected', label: '已拒绝', description: '报价单审批未通过' },
			{ value: 'Expired', label: '已过期', description: '报价单已超过有效期' }
		];
	}
};

// 获取当前用户信息
const loadCurrentUser = async () => {
	try {
		const res = await salQuoteApi.getCurrentUser();
		if (res && res.data && res.data.result) {
			state.ruleForm.approvalUserId = res.data.result.userId;
		}
	} catch (error) {
		console.error('获取当前用户失败:', error);
	}
};

// 判断状态是否可编辑
const isStatusEditable = () => {
	return state.ruleForm.status !== 'Draft';
};

// 页面加载时
onMounted(async () => {
	await loadStatusOptions();
});

// 打开弹窗
const openDialog = async (row: any, title: string) => {
	state.title = title;
	row = row ?? {  };
	isEdit.value = !!row.id;
	customerOptions.value = [];
	
	if (row.id) {
		state.ruleForm = await salQuoteApi.detail(row.id).then(res => res.data.result);
	} else {
		state.ruleForm = JSON.parse(JSON.stringify(row));
		try {
			const quoteNoRes = await salQuoteApi.getNextQuoteNo();
			if (quoteNoRes && quoteNoRes.data && quoteNoRes.data.result) {
				state.ruleForm.quoteNo = quoteNoRes.data.result;
			}
		} catch (error) {
			console.error('获取报价单号失败:', error);
			state.ruleForm.quoteNo = '';
		}
		state.ruleForm.status = 'Draft';
		await loadCurrentUser();
	}
	
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
			await salQuoteApi[state.ruleForm.id ? 'update' : 'add'](values);
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
	<div class="salQuote-container">
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
						<el-form-item label="报价单号" prop="quoteNo">
							<el-input v-model="state.ruleForm.quoteNo" placeholder="系统自动生成" maxlength="50" show-word-limit :disabled="true" />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="客户ID" prop="customerId">
							<el-input v-model="state.ruleForm.customerId" placeholder="选择客户后自动填充" show-word-limit :disabled="true" />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="客户名称">
							<el-select 
								v-model="state.ruleForm.customerId" 
								placeholder="请选择客户" 
								filterable 
								remote
								:remote-method="loadCustomerList"
								:loading="customerLoading"
								@change="handleCustomerChange"
								@visible-change="handleDropdownOpen"
								@clear="handleCustomerChange(null)"
							>
								<el-option 
									v-for="item in customerOptions" 
									:key="item.value" 
									:label="item.label" 
									:value="item.value"
								>
									<span style="display: block; margin-bottom: 4px;">{{ item.label }}</span>
									<span style="font-size: 12px; color: #999;">编码: {{ item.customerCode }}</span>
								</el-option>
							</el-select>
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="报价日期" prop="quoteDate">
							<el-date-picker v-model="state.ruleForm.quoteDate" type="date" placeholder="报价日期" />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="有效开始日期" prop="validStartDate">
							<el-date-picker v-model="state.ruleForm.validStartDate" type="date" placeholder="有效开始日期" />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="有效结束日期" prop="validEndDate">
							<el-date-picker v-model="state.ruleForm.validEndDate" type="date" placeholder="有效结束日期" />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="总金额" prop="totalAmount">
							<el-input-number v-model="state.ruleForm.totalAmount" placeholder="请输入总金额" clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="总税额" prop="totalTaxAmount">
							<el-input-number v-model="state.ruleForm.totalTaxAmount" placeholder="请输入总税额" clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="状态" prop="status">
							<el-select 
								v-model="state.ruleForm.status" 
								placeholder="请选择状态"
								:disabled="state.ruleForm.status === 'Draft'"
							>
								<el-option 
									v-for="item in statusOptions" 
									:key="item.value" 
									:label="item.label" 
									:value="item.value"
								>
									<span style="display: block; margin-bottom: 4px;">{{ item.label }}</span>
									<span style="font-size: 12px; color: #999;">{{ item.description }}</span>
								</el-option>
							</el-select>
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="审批人ID" prop="approvalUserId">
							<el-input v-model="state.ruleForm.approvalUserId" placeholder="系统自动获取当前用户" show-word-limit :disabled="true" />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="审批时间" prop="approvalTime">
							<el-date-picker v-model="state.ruleForm.approvalTime" type="date" placeholder="审批时间" />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="审批备注" prop="approvalRemark">
							<el-input v-model="state.ruleForm.approvalRemark" placeholder="请输入审批备注" maxlength="500" show-word-limit clearable />
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