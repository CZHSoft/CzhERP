<script lang="ts" name="stoStockOut" setup>
import { ref, reactive, onMounted } from "vue";
import { ElMessage } from "element-plus";
import type { FormRules } from "element-plus";
import { formatDate } from '/@/utils/formatTime';
import { useStoStockOutApi } from '/@/api/stock/stoStockOut';
import { getWarehouseList } from '/@/api/stock/stoWarehouse';

const emit = defineEmits(["reloadTable"]);
const stoStockOutApi = useStoStockOutApi();
const ruleFormRef = ref();

const state = reactive({
	title: '',
	loading: false,
	showDialog: false,
	ruleForm: {} as any,
	warehouseList: [] as any[],
});

const stockOutTypeOptions = [
	{ label: '销售出库', value: 'Sale' },
	{ label: '领料出库', value: 'Issue' },
	{ label: '调拨出库', value: 'Transfer' },
	{ label: '其他出库', value: 'Other' },
];

const statusOptions = [
	{ label: '草稿', value: 'Draft' },
	{ label: '已审批', value: 'Approved' },
	{ label: '已确认', value: 'Confirmed' },
	{ label: '已取消', value: 'Cancelled' },
];

const rules = ref<FormRules>({
	stockOutNo: [{required: true, message: '请输入出库单号！', trigger: 'blur',}],
	stockOutType: [{required: true, message: '请选择出库类型！', trigger: 'change',}],
	warehouseId: [{required: true, message: '请选择仓库！', trigger: 'change',}],
	stockOutDate: [{required: true, message: '请选择出库日期！', trigger: 'change',}],
});

onMounted(async () => {
	await loadWarehouseList();
});

const loadWarehouseList = async () => {
	const res = await getWarehouseList();
	state.warehouseList = res.data.result?.items || res.data.result || [];
};

const handleWarehouseChange = async (warehouseId: number) => {
	const warehouse = state.warehouseList.find((w: any) => w.id === warehouseId);
	if (warehouse) {
		state.ruleForm.warehouseCode = warehouse.warehouseCode;
		state.ruleForm.warehouseName = warehouse.warehouseName;
	}
};

const openDialog = async (row: any, title: string) => {
	state.title = title;
	row = row ?? {};
	state.ruleForm = row.id 
		? await stoStockOutApi.detail(row.id).then(res => res.data.result) 
		: JSON.parse(JSON.stringify(row));
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
			await stoStockOutApi[state.ruleForm.id ? 'update' : 'add'](values);
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
	<div class="stoStockOut-container">
		<el-dialog v-model="state.showDialog" :width="700" draggable :close-on-click-modal="false">
			<template #header>
				<div style="color: #fff">
					<span>{{ state.title }}</span>
				</div>
			</template>
			<el-form :model="state.ruleForm" ref="ruleFormRef" label-width="100px" :rules="rules">
				<el-row :gutter="20">
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="出库单号" prop="stockOutNo">
							<el-input v-model="state.ruleForm.stockOutNo" placeholder="请输入出库单号" maxlength="50" show-word-limit clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="出库类型" prop="stockOutType">
							<el-select v-model="state.ruleForm.stockOutType" placeholder="请选择出库类型" style="width: 100%;">
								<el-option v-for="item in stockOutTypeOptions" :key="item.value" :label="item.label" :value="item.value" />
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="仓库" prop="warehouseId">
							<el-select v-model="state.ruleForm.warehouseId" placeholder="请选择仓库" @change="handleWarehouseChange" style="width: 100%;">
								<el-option v-for="item in state.warehouseList" :key="item.id" :label="item.warehouseCode + ' - ' + item.warehouseName" :value="item.id" />
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="仓库编码">
							<el-input v-model="state.ruleForm.warehouseCode" placeholder="仓库编码" :disabled="true" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="来源单据号" prop="sourceBillNo">
							<el-input v-model="state.ruleForm.sourceBillNo" placeholder="请输入来源单据号" maxlength="50" clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="出库日期" prop="stockOutDate">
							<el-date-picker v-model="state.ruleForm.stockOutDate" type="date" placeholder="请选择日期" style="width: 100%;" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="数量">
							<el-input-number v-model="state.ruleForm.totalQuantity" placeholder="请输入数量" :min="0" style="width: 100%;" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="金额">
							<el-input-number v-model="state.ruleForm.totalAmount" placeholder="请输入金额" :min="0" :precision="2" style="width: 100%;" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="状态" prop="status">
							<el-select v-model="state.ruleForm.status" placeholder="请选择状态" style="width: 100%;">
								<el-option v-for="item in statusOptions" :key="item.value" :label="item.label" :value="item.value" />
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="备注">
							<el-input v-model="state.ruleForm.remark" placeholder="请输入备注" type="textarea" :rows="2" maxlength="500" show-word-limit clearable />
						</el-form-item>
					</el-col>
				</el-row>
			</el-form>
			<template #footer>
				<span class="dialog-footer">
					<el-button @click="closeDialog">取 消</el-button>
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