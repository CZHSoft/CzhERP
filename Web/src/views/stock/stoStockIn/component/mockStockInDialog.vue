<script lang="ts" name="mockStockInDialog" setup>
import { ref, reactive, onMounted, watch } from "vue";
import { ElMessage } from "element-plus";
import type { FormRules } from "element-plus";
import { useStoStockInApi } from '/@/api/stock/stoStockIn';
import { getWarehouseList } from '/@/api/stock/stoStockIn';
import { getMaterialList } from '/@/api/stock/stoStockInItem';
import { getLocationList } from '/@/api/stock/stoStockInItem';

const emit = defineEmits(["reloadTable"]);
const stoStockInApi = useStoStockInApi();
const ruleFormRef = ref();

const state = reactive({
	title: '创建批次入库单',
	loading: false,
	showDialog: false,
	ruleForm: {} as any,
	warehouseList: [] as any[],
	materialList: [] as any[],
	locationList: [] as any[],
	items: [] as any[],
});

const stockInTypeOptions = [
	{ label: '采购入库', value: 'Purchase' },
	{ label: '销退入库', value: 'SaleReturn' },
	{ label: '调拨入库', value: 'Transfer' },
	{ label: '其他入库', value: 'Other' },
];

const rules = ref<FormRules>({
  stockInType: [{required: true, message: '请选择入库类型！', trigger: 'change',},],
  warehouseId: [{required: true, message: '请选择仓库！', trigger: 'change',},],
});

onMounted(async () => {
	await loadWarehouseList();
	await loadMaterialList();
});

const loadWarehouseList = async () => {
	const res = await getWarehouseList();
	state.warehouseList = res.data.result || [];
};

const loadMaterialList = async () => {
	const res = await getMaterialList();
	state.materialList = res.data.result || [];
};

const loadLocationList = async (warehouseId: number) => {
	if (!warehouseId) {
		state.locationList = [];
		return;
	}
	const res = await getLocationList(warehouseId);
	state.locationList = res.data.result || [];
};

const handleWarehouseChange = async (warehouseId: number) => {
	const warehouse = state.warehouseList.find((w: any) => w.id === warehouseId);
	if (warehouse) {
		state.ruleForm.warehouseCode = warehouse.warehouseCode;
		state.ruleForm.warehouseName = warehouse.warehouseName;
	}
	await loadLocationList(warehouseId);
};

const openDialog = async () => {
	state.ruleForm = {
		stockInType: 'Purchase',
		warehouseId: null,
		warehouseCode: '',
		warehouseName: '',
		remark: '',
	};
	state.items = [];
	state.locationList = [];
	addItem();
	state.showDialog = true;
};

const addItem = () => {
	state.items.push({
		materialId: null,
		materialCode: '',
		materialName: '',
		spec: '',
		unit: '',
		quantity: 1,
		unitPrice: 0,
		locationCode: '',
		locationId: null,
		batchNo: '',
		expiryDate: null,
		sortOrder: state.items.length + 1,
	});
};

const removeItem = (index: number) => {
	state.items.splice(index, 1);
};

const handleMaterialChange = (index: number) => {
	const material = state.materialList.find((m: any) => m.id === state.items[index].materialId);
	if (material) {
		state.items[index].materialId = material.id;
		state.items[index].materialCode = material.materialCode;
		state.items[index].materialName = material.materialName;
		state.items[index].spec = material.spec || '';
		state.items[index].unit = material.unit || '';
		state.items[index].unitPrice = material.costPrice || 0;
	}
};

const handleLocationChange = (index: number) => {
	const location = state.locationList.find((l: any) => l.id === state.items[index].locationId);
	if (location) {
		state.items[index].locationCode = location.locationCode;
	}
};

const closeDialog = () => {
	emit("reloadTable");
	state.showDialog = false;
};

const submit = async () => {
	ruleFormRef.value.validate(async (isValid: boolean, fields?: any) => {
		if (isValid) {
			if (state.items.length === 0) {
				ElMessage.error('请至少添加一条入库明细！');
				return;
			}
			
			const validItems = state.items.filter(item => item.materialId && item.quantity > 0);
			if (validItems.length === 0) {
				ElMessage.error('请至少填写一条有效的入库明细！');
				return;
			}
			
			state.loading = true;
			try {
				const params = {
					stockInType: state.ruleForm.stockInType,
					warehouseId: state.ruleForm.warehouseId,
					warehouseCode: state.ruleForm.warehouseCode,
					warehouseName: state.ruleForm.warehouseName,
					remark: state.ruleForm.remark,
					items: validItems.map(item => ({
						materialId: item.materialId,
						materialCode: item.materialCode,
						materialName: item.materialName,
						spec: item.spec,
						unit: item.unit,
						quantity: item.quantity,
						unitPrice: item.unitPrice,
						locationCode: item.locationCode,
						batchNo: item.batchNo,
						expiryDate: item.expiryDate,
						sortOrder: item.sortOrder,
					})),
				};
				
				const res = await stoStockInApi.createMockStockIn(params);
				ElMessage.success(res.data.result?.message || '批次入库单创建成功！');
				closeDialog();
			} catch (error) {
				ElMessage.error('创建失败：' + (error as any).message || '未知错误');
			} finally {
				state.loading = false;
			}
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
	<div class="batchStockInDialog-container">
		<el-dialog v-model="state.showDialog" :width="1300" draggable :close-on-click-modal="false" :title="state.title">
			<el-form :model="state.ruleForm" ref="ruleFormRef" label-width="auto" :rules="rules">
				<el-row :gutter="35">
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="入库类型" prop="stockInType">
							<el-select v-model="state.ruleForm.stockInType" placeholder="请选择入库类型">
								<el-option v-for="item in stockInTypeOptions" :key="item.value" :label="item.label" :value="item.value" />
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="仓库" prop="warehouseId">
							<el-select v-model="state.ruleForm.warehouseId" placeholder="请选择仓库" @change="handleWarehouseChange">
								<el-option v-for="item in state.warehouseList" :key="item.id" :label="item.warehouseCode + ' - ' + item.warehouseName" :value="item.id" />
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="仓库编码">
							<el-input v-model="state.ruleForm.warehouseCode" placeholder="根据仓库自动填充" :disabled="true" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="仓库名称">
							<el-input v-model="state.ruleForm.warehouseName" placeholder="根据仓库自动填充" :disabled="true" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="备注">
							<el-input v-model="state.ruleForm.remark" placeholder="请输入备注" maxlength="500" show-word-limit clearable />
						</el-form-item>
					</el-col>
				</el-row>
				
				<el-divider content-position="left">入库明细</el-divider>
				
				<div v-for="(item, index) in state.items" :key="index" class="item-container">
					<el-row :gutter="12">
						<el-col :xs="24" :sm="12" :md="5" :lg="5" :xl="5">
							<el-form-item label="物料">
								<el-select v-model="item.materialId" placeholder="请选择物料" @change="handleMaterialChange(index)">
									<el-option v-for="m in state.materialList" :key="m.id" :label="m.materialCode + ' - ' + m.materialName" :value="m.id" />
								</el-select>
							</el-form-item>
						</el-col>
						<el-col :xs="24" :sm="12" :md="5" :lg="5" :xl="5">
							<el-form-item label="规格">
								<el-input v-model="item.spec" placeholder="规格" :disabled="true" />
							</el-form-item>
						</el-col>
						<el-col :xs="24" :sm="12" :md="3" :lg="3" :xl="3">
							<el-form-item label="单位">
								<el-input v-model="item.unit" placeholder="单位" />
							</el-form-item>
						</el-col>
						<el-col :xs="24" :sm="12" :md="4" :lg="4" :xl="4">
							<el-form-item label="数量">
								<el-input-number v-model="item.quantity" placeholder="数量" :min="1" style="width: 100%;" />
							</el-form-item>
						</el-col>
						<el-col :xs="24" :sm="12" :md="4" :lg="4" :xl="4">
							<el-form-item label="单价">
								<el-input-number v-model="item.unitPrice" placeholder="单价" :min="0" :precision="2" style="width: 100%;" />
							</el-form-item>
						</el-col>
						<el-col :xs="24" :sm="12" :md="3" :lg="3" :xl="3" style="display: flex; align-items: center; justify-content: center;">
							<el-button type="danger" icon="ele-Delete" circle @click="removeItem(index)" :disabled="state.items.length <= 1" />
						</el-col>
					</el-row>
					<el-row :gutter="12" style="margin-top: 10px;">
						<el-col :xs="24" :sm="12" :md="5" :lg="5" :xl="5">
							<el-form-item label="库位">
								<el-select v-model="item.locationId" placeholder="请选择库位" @change="handleLocationChange(index)">
									<el-option v-for="l in state.locationList" :key="l.id" :label="l.locationCode + ' - ' + l.locationName" :value="l.id" />
								</el-select>
							</el-form-item>
						</el-col>
						<el-col :xs="24" :sm="12" :md="5" :lg="5" :xl="5">
							<el-form-item label="库位编码">
								<el-input v-model="item.locationCode" placeholder="库位编码" :disabled="true" />
							</el-form-item>
						</el-col>
						<el-col :xs="24" :sm="12" :md="5" :lg="5" :xl="5">
							<el-form-item label="批号">
								<el-input v-model="item.batchNo" placeholder="请输入批号" />
							</el-form-item>
						</el-col>
						<el-col :xs="24" :sm="12" :md="5" :lg="5" :xl="5">
							<el-form-item label="有效期">
								<el-date-picker v-model="item.expiryDate" type="date" placeholder="选择有效期" />
							</el-form-item>
						</el-col>
					</el-row>
				</div>
				
				<el-button type="primary" icon="ele-Plus" @click="addItem" style="margin-top: 10px;">添加明细</el-button>
			</el-form>
			<template #footer>
				<span class="dialog-footer">
					<el-button @click="closeDialog">取 消</el-button>
					<el-button @click="submit" type="primary" v-reclick="1000" :loading="state.loading">创 建</el-button>
				</span>
			</template>
		</el-dialog>
	</div>
</template>
<style lang="scss" scoped>
.item-container {
	background: #f5f7fa;
	padding: 15px;
	margin-bottom: 10px;
	border-radius: 4px;
}

:deep(.el-select), :deep(.el-input-number) {
  width: 100%;
}

:deep(.el-divider__text) {
	background: #f5f7fa;
}
</style>