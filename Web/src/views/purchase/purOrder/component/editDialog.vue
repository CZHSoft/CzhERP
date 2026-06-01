<script lang="ts" name="purOrder" setup>
import { ref, reactive, onMounted } from "vue";
import { ElMessage } from "element-plus";
import type { FormRules } from "element-plus";
import { formatDate } from '/@/utils/formatTime';
import { usePurOrderApi } from '/@/api/purchase/purOrder';
import { usePurSupplierApi } from '/@/api/purchase/purSupplier';
import { usePurRequisitionApi } from '/@/api/purchase/purRequisition';

const emit = defineEmits(["reloadTable"]);
const purOrderApi = usePurOrderApi();
const purSupplierApi = usePurSupplierApi();
const purRequisitionApi = usePurRequisitionApi();
const ruleFormRef = ref();

const state = reactive({
	title: '',
	loading: false,
	showDialog: false,
	ruleForm: {} as any,
	stores: {},
	dropdownData: {} as any,
	supplierOptions: [] as any[],
	requisitionOptions: [] as any[],
	statusOptions: [
		{ label: '草稿', value: 0 },
		{ label: '审批中', value: 1 },
		{ label: '已确认', value: 2 },
		{ label: '已发货', value: 3 },
		{ label: '已入库', value: 4 },
		{ label: '已完成', value: 5 },
		{ label: '已取消', value: 6 },
	],
});

const rules = ref<FormRules>({
  supplierId: [{required: true, message: '请选择供应商！', trigger: 'blur',},],
  orderDate: [{required: true, message: '请选择下单日期！', trigger: 'blur',},],
});

onMounted(async () => {
	await loadSupplierOptions();
	await loadRequisitionOptions();
});

const loadSupplierOptions = async () => {
	try {
		const res = await purSupplierApi.page({ page: 1, pageSize: 9999 }).then(res => res.data.result);
		state.supplierOptions = res?.items?.map((item: any) => ({
			label: `${item.supplierCode} - ${item.supplierName}`,
			value: item.id,
			supplierCode: item.supplierCode,
			supplierName: item.supplierName,
		})) ?? [];
	} catch (e) {
		console.error('加载供应商列表失败', e);
		state.supplierOptions = [];
	}
};

const loadRequisitionOptions = async () => {
	try {
		const res = await purRequisitionApi.list({ status: 3 }).then(res => res.data.result);
		state.requisitionOptions = res?.map((item: any) => ({
			label: `${item.requisitionNo} - ${item.purpose || ''}`,
			value: item.id,
			requisitionNo: item.requisitionNo,
		})) ?? [];
	} catch (e) {
		console.error('加载采购申请单列表失败', e);
		state.requisitionOptions = [];
	}
};

const handleSupplierChange = (supplierId: number) => {
	const supplier = state.supplierOptions.find((s: any) => s.value === supplierId);
	if (supplier) {
		state.ruleForm.supplierCode = supplier.supplierCode;
		state.ruleForm.supplierName = supplier.supplierName;
	}
};

const handleRequisitionChange = (requisitionId: number) => {
	const requisition = state.requisitionOptions.find((r: any) => r.value === requisitionId);
	if (requisition) {
		state.ruleForm.requisitionNo = requisition.requisitionNo;
	}
};

const openDialog = async (row: any, title: string) => {
	state.title = title;
	state.showDialog = true;
	state.loading = true;
	
	if (row?.id) {
		state.ruleForm = JSON.parse(JSON.stringify(row));
	} else {
		state.ruleForm = {
			id: null,
			orderNo: '',
			supplierId: null,
			supplierCode: '',
			supplierName: '',
			requisitionId: null,
			contractNo: '',
			orderDate: formatDate(new Date(), 'YYYY-mm-dd'),
			deliveryDate: null,
			paymentTerms: '',
			shippingMethod: '',
			totalQty: 0,
			totalAmount: 0,
			taxAmount: 0,
			grandTotal: 0,
			status: 0,
			remark: '',
		};
		const nextCode = await purOrderApi.getNextCode().then(res => res.data.result);
		state.ruleForm.orderNo = nextCode;
	}
	
	state.loading = false;
};

const submit = async () => {
	ruleFormRef.value.validate(async (isValid: boolean, fields?: any) => {
		if (isValid) {
			state.loading = true;
			if (state.ruleForm.id) {
				await purOrderApi.update(state.ruleForm).then(res => {
					ElMessage.success('编辑成功');
					state.showDialog = false;
					emit('reloadTable');
				});
			} else {
				await purOrderApi.add(state.ruleForm).then(res => {
					ElMessage.success('新增成功');
					state.showDialog = false;
					emit('reloadTable');
				});
			}
			state.loading = false;
		}
	});
};

defineExpose({ openDialog });
</script>

<template>
	<div class="purOrder-container">
		<el-dialog v-model="state.showDialog" :width="800" draggable :close-on-click-modal="false">
			<template #header>
				<div style="color: #fff">
					<span>{{ state.title }}</span>
				</div>
			</template>
			<el-form :model="state.ruleForm" ref="ruleFormRef" label-width="auto" :rules="rules" v-loading="state.loading">
				<el-row :gutter="35">
					<el-form-item v-show="false">
						<el-input v-model="state.ruleForm.id" />
					</el-form-item>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="订单号">
							<el-input v-model="state.ruleForm.orderNo" placeholder="系统自动生成" :disabled="true" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="供应商" prop="supplierId">
							<el-select v-model="state.ruleForm.supplierId" placeholder="请选择供应商" clearable filterable @change="handleSupplierChange">
								<el-option v-for="item in state.supplierOptions" :key="item.value" :label="item.label" :value="item.value" />
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="供应商编码">
							<el-input v-model="state.ruleForm.supplierCode" placeholder="自动获取" disabled />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="供应商名称">
							<el-input v-model="state.ruleForm.supplierName" placeholder="自动获取" disabled />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="来源申请单">
							<el-select v-model="state.ruleForm.requisitionId" placeholder="请选择来源申请单（可选）" clearable filterable @change="handleRequisitionChange">
								<el-option v-for="item in state.requisitionOptions" :key="item.value" :label="item.label" :value="item.value" />
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="合同编号">
							<el-input v-model="state.ruleForm.contractNo" placeholder="请输入合同编号" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="下单日期" prop="orderDate">
							<el-date-picker v-model="state.ruleForm.orderDate" type="date" placeholder="请选择下单日期" value-format="YYYY-MM-DD" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="交货日期">
							<el-date-picker v-model="state.ruleForm.deliveryDate" type="date" placeholder="请选择交货日期" value-format="YYYY-MM-DD" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="付款条款">
							<el-input v-model="state.ruleForm.paymentTerms" placeholder="请输入付款条款" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="运输方式">
							<el-input v-model="state.ruleForm.shippingMethod" placeholder="请输入运输方式" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="总数量">
							<el-input-number v-model="state.ruleForm.totalQty" placeholder="请输入总数量" :precision="4" :min="0" disabled />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="总金额(不含税)">
							<el-input-number v-model="state.ruleForm.totalAmount" placeholder="请输入总金额" :precision="4" :min="0" disabled />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="税额">
							<el-input-number v-model="state.ruleForm.taxAmount" placeholder="请输入税额" :precision="4" :min="0" disabled />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="价税合计">
							<el-input-number v-model="state.ruleForm.grandTotal" placeholder="请输入价税合计" :precision="4" :min="0" disabled />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="状态">
							<el-select v-model="state.ruleForm.status" placeholder="请选择状态" :disabled="!!state.ruleForm.id">
								<el-option v-for="item in state.statusOptions" :key="item.value" :label="item.label" :value="item.value" />
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="备注">
							<el-input v-model="state.ruleForm.remark" placeholder="请输入备注" maxlength="500" show-word-limit clearable type="textarea" :rows="3" />
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

<style scoped>
:deep(.el-input), :deep(.el-select), :deep(.el-input-number), :deep(.el-date-picker) {
  width: 100%;
}
</style>