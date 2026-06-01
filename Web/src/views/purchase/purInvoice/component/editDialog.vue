<script lang="ts" name="purInvoice" setup>
import { ref, reactive, onMounted } from "vue";
import { ElMessage } from "element-plus";
import type { FormRules } from "element-plus";
import { formatDate } from '/@/utils/formatTime';
import { usePurInvoiceApi } from '/@/api/purchase/purInvoice';
//父级传递来的函数，用于回调
const emit = defineEmits(["reloadTable"]);
const purInvoiceApi = usePurInvoiceApi();
const ruleFormRef = ref();

const state = reactive({
	title: '',
	loading: false,
	showDialog: false,
	ruleForm: {} as any,
	stores: {},
	dropdownData: {} as any,
});

// 自行添加其他规则
const rules = ref<FormRules>({
  invoiceNo: [{required: true, message: '请选择发票号码！', trigger: 'blur',},],
  supplierId: [{required: true, message: '请选择供应商ID！', trigger: 'blur',},],
  supplierName: [{required: true, message: '请选择供应商名称！', trigger: 'blur',},],
  invoiceType: [{required: true, message: '请选择发票类型(1增值税专票/2增值税普票/3电子发票)！', trigger: 'blur',},],
  invoiceDate: [{required: true, message: '请选择开票日期！', trigger: 'change',},],
  amount: [{required: true, message: '请选择不含税金额！', trigger: 'blur',},],
  taxRate: [{required: true, message: '请选择税率！', trigger: 'blur',},],
  taxAmount: [{required: true, message: '请选择税额！', trigger: 'blur',},],
  grandTotal: [{required: true, message: '请选择价税合计！', trigger: 'blur',},],
  status: [{required: true, message: '请选择状态(0待审核/1已审核/2已核销/3已作废)！', trigger: 'blur',},],
});

// 页面加载时
onMounted(async () => {
});

// 打开弹窗
const openDialog = async (row: any, title: string) => {
	state.title = title;
	row = row ?? {  };
	state.ruleForm = row.id ? await purInvoiceApi.detail(row.id).then(res => res.data.result) : JSON.parse(JSON.stringify(row));
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
			await purInvoiceApi[state.ruleForm.id ? 'update' : 'add'](values);
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
	<div class="purInvoice-container">
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
						<el-form-item label="发票号码" prop="invoiceNo">
							<el-input v-model="state.ruleForm.invoiceNo" placeholder="请输入发票号码" maxlength="50" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="关联订单ID" prop="orderId">
							<el-input v-model="state.ruleForm.orderId" placeholder="请输入关联订单ID" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="关联入库单ID" prop="inboundId">
							<el-input v-model="state.ruleForm.inboundId" placeholder="请输入关联入库单ID" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="供应商ID" prop="supplierId">
							<el-input v-model="state.ruleForm.supplierId" placeholder="请输入供应商ID" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="供应商名称" prop="supplierName">
							<el-input v-model="state.ruleForm.supplierName" placeholder="请输入供应商名称" maxlength="100" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="发票类型(1增值税专票/2增值税普票/3电子发票)" prop="invoiceType">
							<el-input-number v-model="state.ruleForm.invoiceType" placeholder="请输入发票类型(1增值税专票/2增值税普票/3电子发票)" clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="开票日期" prop="invoiceDate">
							<el-date-picker v-model="state.ruleForm.invoiceDate" type="date" placeholder="开票日期" />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="不含税金额" prop="amount">
							<el-input-number v-model="state.ruleForm.amount" placeholder="请输入不含税金额" clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="税率" prop="taxRate">
							<el-input-number v-model="state.ruleForm.taxRate" placeholder="请输入税率" clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="税额" prop="taxAmount">
							<el-input-number v-model="state.ruleForm.taxAmount" placeholder="请输入税额" clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="价税合计" prop="grandTotal">
							<el-input-number v-model="state.ruleForm.grandTotal" placeholder="请输入价税合计" clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="状态(0待审核/1已审核/2已核销/3已作废)" prop="status">
							<el-input-number v-model="state.ruleForm.status" placeholder="请输入状态(0待审核/1已审核/2已核销/3已作废)" clearable />
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