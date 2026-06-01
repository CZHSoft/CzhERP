<script lang="ts" name="stoTransferItem" setup>
import { ref, reactive, onMounted } from "vue";
import { ElMessage } from "element-plus";
import type { FormRules } from "element-plus";
import { formatDate } from '/@/utils/formatTime';
import { useStoTransferItemApi } from '/@/api/stock/stoTransferItem';
import { getTransferList } from '/@/api/stock/stoTransfer';
import { getMaterialList } from '/@/api/base/basMaterial';
//父级传递来的函数，用于回调
const emit = defineEmits(["reloadTable"]);
const stoTransferItemApi = useStoTransferItemApi();
const ruleFormRef = ref();

const state = reactive({
	title: '',
	loading: false,
	showDialog: false,
	ruleForm: {} as any,
	stores: {},
	dropdownData: {
		transferList: [] as any[],
		materialList: [] as any[],
	},
});

// 自行添加其他规则
const rules = ref<FormRules>({
  transferNo: [{required: true, message: '请选择调拨单！', trigger: 'change',},],
  materialName: [{required: true, message: '请选择物料！', trigger: 'change',},],
  quantity: [{required: true, message: '请输入调拨数量！', trigger: 'blur',},],
});

// 页面加载时
onMounted(async () => {
	await Promise.all([loadTransferList(), loadMaterialList()]);
});

// 加载调拨单列表
const loadTransferList = async () => {
	const res = await getTransferList({ status: 'Draft' });
	state.dropdownData.transferList = res.data.result.items || [];
};

// 加载物料列表
const loadMaterialList = async () => {
	const res = await getMaterialList();
	state.dropdownData.materialList = res.data.result || [];
};

// 调拨单选择变更
const handleTransferChange = (item: any) => {
	if (item) {
		state.ruleForm.transferId = item.id;
		state.ruleForm.transferNo = item.transferNo;
	} else {
		state.ruleForm.transferId = '';
		state.ruleForm.transferNo = '';
	}
};

// 物料选择变更
const handleMaterialChange = (item: any) => {
	if (item) {
		state.ruleForm.materialId = item.id;
		state.ruleForm.materialCode = item.materialCode;
		state.ruleForm.materialName = item.materialName;
		state.ruleForm.spec = item.spec;
		state.ruleForm.unit = item.unit;
	} else {
		state.ruleForm.materialId = '';
		state.ruleForm.materialCode = '';
		state.ruleForm.materialName = '';
		state.ruleForm.spec = '';
		state.ruleForm.unit = '';
	}
};

// 打开弹窗
const openDialog = async (row: any, title: string) => {
	state.title = title;
	row = row ?? {  };
	if (row.id) {
		state.ruleForm = await stoTransferItemApi.detail(row.id).then(res => res.data.result);
	} else {
		state.ruleForm = JSON.parse(JSON.stringify(row));
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
			await stoTransferItemApi[state.ruleForm.id ? 'update' : 'add'](values);
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
	<div class="stoTransferItem-container">
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
						<el-form-item label="调拨单" prop="transferNo">
							<el-select 
								v-model="state.ruleForm.transferNo" 
								placeholder="请选择调拨单" 
								clearable
								@change="(val) => handleTransferChange(state.dropdownData.transferList.find(t => t.transferNo === val))">
								<el-option 
									v-for="item in state.dropdownData.transferList" 
									:key="item.id" 
									:label="item.transferNo" 
									:value="item.transferNo" />
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="物料名称" prop="materialName">
							<el-select 
								v-model="state.ruleForm.materialName" 
								placeholder="请选择物料" 
								clearable
								@change="(val) => handleMaterialChange(state.dropdownData.materialList.find(m => m.materialName === val))">
								<el-option 
									v-for="item in state.dropdownData.materialList" 
									:key="item.id" 
									:label="item.materialName" 
									:value="item.materialName" />
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="规格型号" prop="spec">
							<el-input v-model="state.ruleForm.spec" placeholder="自动填充" :disabled="true" maxlength="100" show-word-limit />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="单位" prop="unit">
							<el-input v-model="state.ruleForm.unit" placeholder="自动填充" :disabled="true" maxlength="20" show-word-limit />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="调拨数量" prop="quantity">
							<el-input-number v-model="state.ruleForm.quantity" placeholder="请输入调拨数量" clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="转出库位" prop="fromLocationCode">
							<el-input v-model="state.ruleForm.fromLocationCode" placeholder="请输入转出库位" maxlength="50" show-word-limit clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="转入库位" prop="toLocationCode">
							<el-input v-model="state.ruleForm.toLocationCode" placeholder="请输入转入库位" maxlength="50" show-word-limit clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="批号" prop="batchNo">
							<el-input v-model="state.ruleForm.batchNo" placeholder="请输入批号" maxlength="50" show-word-limit clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="排序号" prop="sortOrder">
							<el-input-number v-model="state.ruleForm.sortOrder" placeholder="请输入排序号" :min="1" clearable />
						</el-form-item>
					</el-col>
					<!-- 隐藏字段 -->
					<el-form-item v-show="false" prop="transferId">
						<el-input v-model="state.ruleForm.transferId" />
					</el-form-item>
					<el-form-item v-show="false" prop="materialId">
						<el-input v-model="state.ruleForm.materialId" />
					</el-form-item>
					<el-form-item v-show="false" prop="materialCode">
						<el-input v-model="state.ruleForm.materialCode" />
					</el-form-item>
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