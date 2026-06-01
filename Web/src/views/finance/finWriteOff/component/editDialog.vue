<script lang="ts" name="finWriteOff" setup>
import { ref, reactive, onMounted } from "vue";
import { ElMessage } from "element-plus";
import type { FormRules } from "element-plus";
import { formatDate } from '/@/utils/formatTime';
import { useFinWriteOffApi } from '/@/api/finance/finWriteOff';

const emit = defineEmits(["reloadTable"]);
const finWriteOffApi = useFinWriteOffApi();
const ruleFormRef = ref();

const state = reactive({
	title: '',
	loading: false,
	showDialog: false,
	ruleForm: {} as any,
	stores: {},
	dropdownData: {} as any,
	writeOffTypeOptions: [] as any[],
	businessTypeOptions: [] as any[],
	statusOptions: [] as any[],
});

const rules = ref<FormRules>({
  writeOffType: [{required: true, message: '请选择核销类型！', trigger: 'change',},],
  businessType: [{required: true, message: '请选择业务类型！', trigger: 'change',},],
  businessId: [{required: true, message: '请选择关联单据ID！', trigger: 'blur',},],
  businessNo: [{required: true, message: '请选择关联单据号！', trigger: 'blur',},],
  writeOffAmount: [{required: true, message: '请选择核销金额！', trigger: 'blur',},],
  remainAmount: [{required: true, message: '请选择剩余金额！', trigger: 'blur',},],
  writeOffDate: [{required: true, message: '请选择核销日期！', trigger: 'change',},],
  status: [{required: true, message: '请选择状态！', trigger: 'change',},],
});

onMounted(async () => {
	await loadDropdownData();
});

const loadDropdownData = async () => {
	try {
		const writeOffTypeRes = await finWriteOffApi.getWriteOffTypes();
		state.writeOffTypeOptions = writeOffTypeRes.data.result || [];
		
		const businessTypeRes = await finWriteOffApi.getBusinessTypes();
		state.businessTypeOptions = businessTypeRes.data.result || [];
		
		const statusRes = await finWriteOffApi.getStatuses();
		state.statusOptions = statusRes.data.result || [];
	} catch (error) {
		console.error('加载下拉数据失败:', error);
	}
};

const openDialog = async (row: any, title: string) => {
	state.title = title;
	row = row ?? {};
	if (!row.id) {
		try {
			const res = await finWriteOffApi.getNewWriteOffNo();
			row.writeOffNo = res.data.result;
		} catch (error) {
			console.error('获取核销单号失败:', error);
		}
	}
	state.ruleForm = row.id ? await finWriteOffApi.detail(row.id).then(res => res.data.result) : JSON.parse(JSON.stringify(row));
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
			await finWriteOffApi[state.ruleForm.id ? 'update' : 'add'](values);
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
	<div class="finWriteOff-container">
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
						<el-form-item label="核销单号" prop="writeOffNo">
							<el-input v-model="state.ruleForm.writeOffNo" placeholder="系统自动生成" maxlength="50" show-word-limit clearable disabled />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="核销类型" prop="writeOffType">
							<el-select v-model="state.ruleForm.writeOffType" clearable filterable placeholder="请选择核销类型" style="width: 100%">
								<el-option v-for="item in state.writeOffTypeOptions" :key="item.code" :label="item.name" :value="item.code" />
							</el-select>
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="业务类型" prop="businessType">
							<el-select v-model="state.ruleForm.businessType" clearable filterable placeholder="请选择业务类型" style="width: 100%">
								<el-option v-for="item in state.businessTypeOptions" :key="item.code" :label="item.name" :value="item.code" />
							</el-select>
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="关联单据ID" prop="businessId">
							<el-input v-model="state.ruleForm.businessId" placeholder="请输入关联单据ID" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="关联单据号" prop="businessNo">
							<el-input v-model="state.ruleForm.businessNo" placeholder="请输入关联单据号" maxlength="50" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="核销金额" prop="writeOffAmount">
							<el-input-number v-model="state.ruleForm.writeOffAmount" placeholder="请输入核销金额" clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="剩余金额" prop="remainAmount">
							<el-input-number v-model="state.ruleForm.remainAmount" placeholder="请输入剩余金额" clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="核销日期" prop="writeOffDate">
							<el-date-picker v-model="state.ruleForm.writeOffDate" type="date" placeholder="核销日期" />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="核销人ID" prop="writeOffUserId">
							<el-input v-model="state.ruleForm.writeOffUserId" placeholder="请输入核销人ID" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="核销人姓名" prop="writeOffUserName">
							<el-input v-model="state.ruleForm.writeOffUserName" placeholder="请输入核销人姓名" maxlength="50" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="状态" prop="status">
							<el-select v-model="state.ruleForm.status" clearable filterable placeholder="请选择状态" style="width: 100%">
								<el-option v-for="item in state.statusOptions" :key="item.code" :label="item.name" :value="item.code" />
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