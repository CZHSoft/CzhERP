<script lang="ts" name="purRequisition" setup>
import { ref, reactive, onMounted } from "vue";
import { ElMessage } from "element-plus";
import type { FormRules } from "element-plus";
import { formatDate } from '/@/utils/formatTime';
import { usePurRequisitionApi } from '/@/api/purchase/purRequisition';
import { getAPI } from '/@/utils/axios-utils';
import { SysOrgApi, SysUserApi } from '/@/api-services/api';

const emit = defineEmits(["reloadTable"]);
const purRequisitionApi = usePurRequisitionApi();
const ruleFormRef = ref();

const state = reactive({
	title: '',
	loading: false,
	showDialog: false,
	ruleForm: {} as any,
	stores: {},
	dropdownData: {} as any,
	orgOptions: [] as any[],
	userOptions: [] as any[],
	statusOptions: [
		{ label: '草稿', value: 0 },
		{ label: '提交', value: 1 },
		{ label: '审批中', value: 2 },
		{ label: '通过', value: 3 },
		{ label: '拒绝', value: 4 },
	],
});

const cascaderProps = { checkStrictly: true, emitPath: false, value: 'id', label: 'name', expandTrigger: 'hover' };

const rules = ref<FormRules>({
  departmentId: [{required: true, message: '请选择申请部门！', trigger: 'blur',}],
  applicantId: [{required: true, message: '请选择申请人！', trigger: 'blur',}],
  applyDate: [{required: true, message: '请选择申请日期！', trigger: 'blur',}],
});

onMounted(async () => {
	await loadOrgOptions();
	await loadUserOptions();
});

const loadOrgOptions = async () => {
	try {
		const res = await getAPI(SysOrgApi).apiSysOrgTreeGet(0);
		state.orgOptions = res.data.result ?? [];
	} catch (e) {
		console.error('加载部门列表失败', e);
		state.orgOptions = [];
	}
};

const loadUserOptions = async () => {
	try {
		const res = await getAPI(SysUserApi).apiSysUserPagePost({ page: 1, pageSize: 9999 });
		state.userOptions = res.data.result?.items?.map((item: any) => ({
			label: item.realName || item.account,
			value: item.id,
			orgId: item.orgId
		})) ?? [];
	} catch (e) {
		console.error('加载用户列表失败', e);
		state.userOptions = [];
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
			requisitionNo: '',
			departmentId: null,
			applicantId: null,
			applyDate: formatDate(new Date(), 'YYYY-mm-dd'),
			expectedDate: null,
			totalAmount: 0,
			status: 0,
			purpose: '',
			remark: '',
		};
		const nextCode = await purRequisitionApi.getNextCode().then(res => res.data.result);
		state.ruleForm.requisitionNo = nextCode;
	}
	
	state.loading = false;
};

const submit = async () => {
	ruleFormRef.value.validate(async (isValid: boolean, fields?: any) => {
		if (isValid) {
			state.loading = true;
			if (state.ruleForm.id) {
				await purRequisitionApi.update(state.ruleForm).then(res => {
					ElMessage.success('编辑成功');
					state.showDialog = false;
					emit('reloadTable');
				});
			} else {
				await purRequisitionApi.add(state.ruleForm).then(res => {
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
	<div class="purRequisition-container">
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
						<el-form-item label="申请单号">
							<el-input v-model="state.ruleForm.requisitionNo" placeholder="系统自动生成" :disabled="true" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="申请部门" prop="departmentId">
							<el-cascader 
								v-model="state.ruleForm.departmentId" 
								:options="state.orgOptions" 
								:props="cascaderProps" 
								placeholder="请选择申请部门" 
								clearable 
								filterable 
								class="w100" 
							>
								<template #default="{ node, data }">
									<span>{{ data.name }}</span>
									<span v-if="!node.isLeaf"> ({{ data.children?.length || 0 }}) </span>
								</template>
							</el-cascader>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="申请人" prop="applicantId">
							<el-select v-model="state.ruleForm.applicantId" placeholder="请选择申请人" clearable filterable class="w100">
								<el-option v-for="item in state.userOptions" :key="item.value" :label="item.label" :value="item.value" />
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="申请日期" prop="applyDate">
							<el-date-picker v-model="state.ruleForm.applyDate" type="date" placeholder="请选择申请日期" value-format="YYYY-MM-DD" class="w100" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="期望到货日期">
							<el-date-picker v-model="state.ruleForm.expectedDate" type="date" placeholder="请选择期望到货日期" value-format="YYYY-MM-DD" class="w100" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="总金额">
							<el-input-number v-model="state.ruleForm.totalAmount" placeholder="请输入总金额" :precision="4" :min="0" disabled class="w100" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="状态">
							<el-select v-model="state.ruleForm.status" placeholder="请选择状态" disabled class="w100">
								<el-option v-for="item in state.statusOptions" :key="item.value" :label="item.label" :value="item.value" />
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="用途说明">
							<el-input v-model="state.ruleForm.purpose" placeholder="请输入用途说明" maxlength="500" show-word-limit clearable type="textarea" :rows="3" />
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
.w100 {
  width: 100%;
}
</style>