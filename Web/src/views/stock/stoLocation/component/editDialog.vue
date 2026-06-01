<script lang="ts" name="stoLocation" setup>
import { ref, reactive, onMounted } from "vue";
import { ElMessage } from "element-plus";
import type { FormRules } from "element-plus";
import { formatDate } from '/@/utils/formatTime';
import { useStoLocationApi, getWarehouseList } from '/@/api/stock/stoLocation';
const emit = defineEmits(["reloadTable"]);
const stoLocationApi = useStoLocationApi();
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

const isEnabledOptions = [
	{ label: '是', value: 1 },
	{ label: '否', value: 0 },
];

const rules = ref<FormRules>({
  warehouseId: [{required: true, message: '请选择仓库！', trigger: 'change',},],
  warehouseCode: [{required: true, message: '请选择仓库编码！', trigger: 'change',},],
  locationCode: [{required: true, message: '库位编码由系统自动生成！', trigger: 'blur',},],
  capacity: [{required: true, message: '请输入库位容量！', trigger: 'blur',},],
  isEnabled: [{required: true, message: '请选择是否启用！', trigger: 'change',},],
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
	}
};

const openDialog = async (row: any, title: string) => {
	state.title = title;
	row = row ?? {  };
	if (row.id) {
		state.ruleForm = await stoLocationApi.detail(row.id).then(res => res.data.result);
	} else {
		state.ruleForm = JSON.parse(JSON.stringify(row));
		state.ruleForm.isEnabled = 1;
		const codeRes = await stoLocationApi.getNextCode();
		state.ruleForm.locationCode = codeRes.data.result;
		state.ruleForm.warehouseId = null;
		state.ruleForm.warehouseCode = '';
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
			await stoLocationApi[state.ruleForm.id ? 'update' : 'add'](values);
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
	<div class="stoLocation-container">
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
						<el-form-item label="库位编码" prop="locationCode">
							<el-input v-model="state.ruleForm.locationCode" placeholder="系统自动生成" :disabled="true" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="库位名称" prop="locationName">
							<el-input v-model="state.ruleForm.locationName" placeholder="请输入库位名称" maxlength="100" show-word-limit clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="库位容量" prop="capacity">
							<el-input-number v-model="state.ruleForm.capacity" placeholder="请输入库位容量" clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="是否启用" prop="isEnabled">
							<el-select v-model="state.ruleForm.isEnabled" placeholder="请选择是否启用">
								<el-option v-for="item in isEnabledOptions" :key="item.value" :label="item.label" :value="item.value" />
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="备注" prop="remark">
							<el-input v-model="state.ruleForm.remark" placeholder="请输入备注" maxlength="200" show-word-limit clearable />
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
