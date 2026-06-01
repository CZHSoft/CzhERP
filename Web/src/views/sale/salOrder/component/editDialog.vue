<script lang="ts" name="salOrder" setup>
import { ref, reactive, onMounted } from "vue";
import { ElMessage } from "element-plus";
import type { FormRules } from "element-plus";
import { formatDate } from '/@/utils/formatTime';
import { useSalOrderApi } from '/@/api/sale/salOrder';
//父级传递来的函数，用于回调
const emit = defineEmits(["reloadTable"]);
const salOrderApi = useSalOrderApi();
const ruleFormRef = ref();

const state = reactive({
	title: '',
	loading: false,
	showDialog: false,
	ruleForm: {} as any,
	stores: {},
	dropdownData: {} as any,
	statusOptions: [] as any[],
});

// 自行添加其他规则
const rules = ref<FormRules>({
  orderNo: [{required: true, message: '请选择订单号！', trigger: 'blur',},],
  customerId: [{required: true, message: '请选择客户ID！', trigger: 'blur',},],
  customerName: [{required: true, message: '请选择客户名称！', trigger: 'blur',},],
  orderDate: [{required: true, message: '请选择下单日期！', trigger: 'change',},],
  shippingFee: [{required: true, message: '请选择运费！', trigger: 'blur',},],
  totalAmount: [{required: true, message: '请选择总金额！', trigger: 'blur',},],
  totalTaxAmount: [{required: true, message: '请选择总税额！', trigger: 'blur',},],
  totalDiscount: [{required: true, message: '请选择总折扣！', trigger: 'blur',},],
  payAmount: [{required: true, message: '请选择已付款金额！', trigger: 'blur',},],
  status: [{required: true, message: '请选择状态！', trigger: 'change',},],
  creditUsedAmount: [{required: true, message: '请选择本次使用信用额度！', trigger: 'blur',},],
});

// 页面加载时
onMounted(async () => {
  await loadStatusOptions();
});

const loadStatusOptions = async () => {
  try {
    const result = await salOrderApi.getStatusOptions();
    state.statusOptions = result.data?.result || result.data || result;
  } catch (error) {
    console.error('加载状态选项失败:', error);
  }
};

// 打开弹窗
const openDialog = async (row: any, title: string) => {
	state.title = title;
	row = row ?? {  };
	state.ruleForm = row.id ? await salOrderApi.detail(row.id).then(res => res.data.result) : JSON.parse(JSON.stringify(row));
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
			await salOrderApi[state.ruleForm.id ? 'update' : 'add'](values);
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
	<div class="salOrder-container">
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
						<el-form-item label="订单号" prop="orderNo">
							<el-input v-model="state.ruleForm.orderNo" placeholder="请输入订单号" maxlength="50" show-word-limit clearable :disabled="!!state.ruleForm.id" />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="客户ID" prop="customerId">
							<el-input v-model="state.ruleForm.customerId" placeholder="请输入客户ID" show-word-limit clearable :disabled="!!state.ruleForm.id" />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="客户名称" prop="customerName">
							<el-input v-model="state.ruleForm.customerName" placeholder="请输入客户名称" maxlength="100" show-word-limit clearable :disabled="!!state.ruleForm.id" />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="联系人姓名" prop="contactName">
							<el-input v-model="state.ruleForm.contactName" placeholder="请输入联系人姓名" maxlength="50" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="联系电话" prop="contactPhone">
							<el-input v-model="state.ruleForm.contactPhone" placeholder="请输入联系电话" maxlength="20" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="下单日期" prop="orderDate">
							<el-date-picker v-model="state.ruleForm.orderDate" type="date" placeholder="下单日期" />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="预计交货日期" prop="deliveryDate">
							<el-date-picker v-model="state.ruleForm.deliveryDate" type="date" placeholder="预计交货日期" />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="送货地址" prop="address">
							<el-input v-model="state.ruleForm.address" placeholder="请输入送货地址" maxlength="200" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="配送方式" prop="shippingMethod">
							<el-input v-model="state.ruleForm.shippingMethod" placeholder="请输入配送方式" maxlength="50" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="运费" prop="shippingFee">
							<el-input-number v-model="state.ruleForm.shippingFee" placeholder="请输入运费" clearable />
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
						<el-form-item label="总折扣" prop="totalDiscount">
							<el-input-number v-model="state.ruleForm.totalDiscount" placeholder="请输入总折扣" clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="已付款金额" prop="payAmount">
							<el-input-number v-model="state.ruleForm.payAmount" placeholder="请输入已付款金额" clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="付款方式" prop="paymentType">
							<el-input v-model="state.ruleForm.paymentType" placeholder="请输入付款方式" maxlength="20" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="状态" prop="status">
							<el-select v-model="state.ruleForm.status" placeholder="请选择状态">
								<el-option v-for="option in state.statusOptions" :key="option.value" :label="option.label" :value="option.value" />
							</el-select>
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="信用检查结果" prop="creditCheckResult">
							<el-input v-model="state.ruleForm.creditCheckResult" placeholder="请输入信用检查结果" maxlength="20" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="本次使用信用额度" prop="creditUsedAmount">
							<el-input-number v-model="state.ruleForm.creditUsedAmount" placeholder="请输入本次使用信用额度" clearable />
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