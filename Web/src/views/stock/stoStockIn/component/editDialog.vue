<script lang="ts" name="stoStockIn" setup>
import { ref, reactive, onMounted } from "vue";
import { ElMessage } from "element-plus";
import type { FormRules } from "element-plus";
import { formatDate } from '/@/utils/formatTime';
import { useStoStockInApi, getWarehouseList } from '/@/api/stock/stoStockIn';
const emit = defineEmits(["reloadTable"]);
const stoStockInApi = useStoStockInApi();
const ruleFormRef = ref();

const state = reactive({
	title: '',
	loading: false,
	showDialog: false,
	ruleForm: {} as any,
	stores: {},
	dropdownData: {} as any,
	warehouseList: [] as any[],
});

const stockInTypeOptions = [
	{ label: '采购入库', value: 'Purchase' },
	{ label: '销退入库', value: 'SaleReturn' },
	{ label: '调拨入库', value: 'Transfer' },
	{ label: '其他入库', value: 'Other' },
];

const statusOptions = [
	{ label: '草稿', value: 'Draft' },
	{ label: '已审批', value: 'Approved' },
];

const rules = ref<FormRules>({
  stockInNo: [{required: true, message: '入库单号由系统自动生成！', trigger: 'blur',},],
  stockInType: [{required: true, message: '请选择入库类型！', trigger: 'change',},],
  warehouseId: [{required: true, message: '请选择仓库！', trigger: 'change',},],
  warehouseCode: [{required: true, message: '请选择仓库编码！', trigger: 'change',},],
  stockInDate: [{required: true, message: '请选择入库日期！', trigger: 'change',},],
  totalQuantity: [{required: true, message: '请输入入库总数量！', trigger: 'blur',},],
  totalAmount: [{required: true, message: '请输入入库总金额！', trigger: 'blur',},],
  status: [{required: true, message: '请选择状态！', trigger: 'change',},],
});

onMounted(async () => {
	await loadWarehouseList();
});

const loadWarehouseList = async () => {
	const res = await getWarehouseList();
	state.warehouseList = res.data.result || [];
};

const handleWarehouseChange = (warehouseCode: string) => {
	const warehouse = state.warehouseList.find((w: any) => w.warehouseCode === warehouseCode);
	if (warehouse) {
		state.ruleForm.warehouseId = warehouse.id;
		state.ruleForm.warehouseName = warehouse.warehouseName;
	}
};

const openDialog = async (row: any, title: string) => {
	state.title = title;
	row = row ?? {  };
	if (row.id) {
		state.ruleForm = await stoStockInApi.detail(row.id).then(res => res.data.result);
	} else {
		state.ruleForm = JSON.parse(JSON.stringify(row));
		state.ruleForm.status = 'Draft';
		const codeRes = await stoStockInApi.getNextCode();
		state.ruleForm.stockInNo = codeRes.data.result;
		state.ruleForm.stockInDate = new Date();
		state.ruleForm.warehouseId = null;
		state.ruleForm.warehouseCode = '';
		state.ruleForm.warehouseName = '';
		state.ruleForm.totalQuantity = 0;
		state.ruleForm.totalAmount = 0;
	}
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
			await stoStockInApi[state.ruleForm.id ? 'update' : 'add'](values);
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
	<div class="stoStockIn-container">
		<el-dialog v-model="state.showDialog" :width="900" draggable :close-on-click-modal="false">
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
						<el-form-item label="入库单号" prop="stockInNo">
							<el-input v-model="state.ruleForm.stockInNo" placeholder="系统自动生成" :disabled="true" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="入库类型" prop="stockInType">
							<el-select v-model="state.ruleForm.stockInType" placeholder="请选择入库类型">
								<el-option v-for="item in stockInTypeOptions" :key="item.value" :label="item.label" :value="item.value" />
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="仓库ID" prop="warehouseId">
							<el-input v-model="state.ruleForm.warehouseId" placeholder="系统自动填写" :disabled="true" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="仓库编码" prop="warehouseCode">
							<el-select v-model="state.ruleForm.warehouseCode" placeholder="请选择仓库" @change="handleWarehouseChange">
								<el-option v-for="item in state.warehouseList" :key="item.id" :label="item.warehouseCode + ' - ' + item.warehouseName" :value="item.warehouseCode" />
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="仓库名称" prop="warehouseName">
							<el-input v-model="state.ruleForm.warehouseName" placeholder="根据仓库编码自动填充" :disabled="true" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="来源单据号" prop="sourceBillNo">
							<el-input v-model="state.ruleForm.sourceBillNo" placeholder="请输入来源单据号" maxlength="50" show-word-limit clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="入库日期" prop="stockInDate">
							<el-date-picker v-model="state.ruleForm.stockInDate" type="date" placeholder="请选择入库日期" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="入库总数量" prop="totalQuantity">
							<el-input-number v-model="state.ruleForm.totalQuantity" placeholder="请输入入库总数量" clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="入库总金额" prop="totalAmount">
							<el-input-number v-model="state.ruleForm.totalAmount" placeholder="请输入入库总金额" clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="状态" prop="status">
							<el-select v-model="state.ruleForm.status" placeholder="请选择状态">
								<el-option v-for="item in statusOptions" :key="item.value" :label="item.label" :value="item.value" />
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
						<el-form-item label="审批意见" prop="approvalRemark">
							<el-input v-model="state.ruleForm.approvalRemark" placeholder="请输入审批意见" maxlength="500" show-word-limit clearable />
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
