<script lang="ts" name="stoWarehouse" setup>
import { ref, reactive, onMounted } from "vue";
import { ElMessage } from "element-plus";
import type { FormRules } from "element-plus";
import { formatDate } from '/@/utils/formatTime';
import { useStoWarehouseApi } from '/@/api/stock/stoWarehouse';
const emit = defineEmits(["reloadTable"]);
const stoWarehouseApi = useStoWarehouseApi();
const ruleFormRef = ref();

const state = reactive({
	title: '',
	loading: false,
	showDialog: false,
	ruleForm: {} as any,
	stores: {},
	dropdownData: {} as any,
});

const warehouseTypeOptions = [
	{ label: '普通仓', value: 'Normal' },
	{ label: '保税仓', value: 'Bonded' },
	{ label: '退货仓', value: 'Return' },
];

const isEnabledOptions = [
	{ label: '是', value: 1 },
	{ label: '否', value: 0 },
];

const rules = ref<FormRules>({
  warehouseCode: [{required: true, message: '仓库编码由系统自动生成！', trigger: 'blur',},],
  warehouseName: [{required: true, message: '请输入仓库名称！', trigger: 'blur',},],
  warehouseType: [{required: true, message: '请选择仓库类型！', trigger: 'change',},],
  capacity: [{required: true, message: '请输入仓库容量！', trigger: 'blur',},],
  isEnabled: [{required: true, message: '请选择是否启用！', trigger: 'blur',},],
});

onMounted(async () => {
});

const openDialog = async (row: any, title: string) => {
	state.title = title;
	row = row ?? {  };
	if (row.id) {
		state.ruleForm = await stoWarehouseApi.detail(row.id).then(res => res.data.result);
	} else {
		state.ruleForm = JSON.parse(JSON.stringify(row));
		state.ruleForm.isEnabled = 1;
		const codeRes = await stoWarehouseApi.getNextCode();
		state.ruleForm.warehouseCode = codeRes.data.result;
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
			await stoWarehouseApi[state.ruleForm.id ? 'update' : 'add'](values);
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
	<div class="stoWarehouse-container">
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
						<el-form-item label="仓库编码" prop="warehouseCode">
							<el-input v-model="state.ruleForm.warehouseCode" placeholder="系统自动生成" maxlength="50" show-word-limit clearable :disabled="true" />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="仓库名称" prop="warehouseName">
							<el-input v-model="state.ruleForm.warehouseName" placeholder="请输入仓库名称" maxlength="100" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="仓库类型" prop="warehouseType">
							<el-select v-model="state.ruleForm.warehouseType" placeholder="请选择仓库类型">
								<el-option v-for="item in warehouseTypeOptions" :key="item.value" :label="item.label" :value="item.value" />
							</el-select>
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="仓库地址" prop="address">
							<el-input v-model="state.ruleForm.address" placeholder="请输入仓库地址" maxlength="200" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="省份" prop="province">
							<el-input v-model="state.ruleForm.province" placeholder="请输入省份" maxlength="50" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="城市" prop="city">
							<el-input v-model="state.ruleForm.city" placeholder="请输入城市" maxlength="50" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="仓库负责人" prop="contactName">
							<el-input v-model="state.ruleForm.contactName" placeholder="请输入仓库负责人" maxlength="50" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="联系电话" prop="contactPhone">
							<el-input v-model="state.ruleForm.contactPhone" placeholder="请输入联系电话" maxlength="20" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="仓库容量" prop="capacity">
							<el-input-number v-model="state.ruleForm.capacity" placeholder="请输入仓库容量" clearable />
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
