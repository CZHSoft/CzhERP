<script lang="ts" name="finAccount" setup>
import { ref, reactive, onMounted } from "vue";
import { ElMessage } from "element-plus";
import type { FormRules } from "element-plus";
import { formatDate } from '/@/utils/formatTime';
import { useFinAccountApi } from '/@/api/finance/finAccount';

//父级传递来的函数，用于回调
const emit = defineEmits(["reloadTable"]);
const finAccountApi = useFinAccountApi();
const ruleFormRef = ref();

const state = reactive({
	title: '',
	loading: false,
	showDialog: false,
	ruleForm: {} as any,
	stores: {},
	dropdownData: {} as any,
	accountTypes: [] as any[], // 科目类型下拉数据
	directions: [] as any[], // 余额方向下拉数据
	accountSelector: [] as any[], // 科目下拉数据
});

// 自行添加其他规则
const rules = ref<FormRules>({
	accountCode: [{ required: true, message: '请输入科目编码', trigger: 'blur' }],
	accountName: [{ required: true, message: '请输入科目名称', trigger: 'blur' }],
	level: [{ required: true, message: '请选择科目级次', trigger: 'change' }],
	accountType: [{ required: true, message: '请选择科目类型', trigger: 'change' }],
	direction: [{ required: true, message: '请选择余额方向', trigger: 'change' }],
	isDetail: [{ required: true, message: '请选择是否明细科目', trigger: 'change' }],
	isAuxiliary: [{ required: true, message: '请选择是否辅助核算', trigger: 'change' }],
	isCashFlow: [{ required: true, message: '请选择是否现金流量科目', trigger: 'change' }],
	isEnabled: [{ required: true, message: '请选择是否启用', trigger: 'change' }],
	sortOrder: [{ required: true, message: '请输入排序号', trigger: 'blur' }],
});

// 页面加载时
onMounted(async () => {
	await loadDropdownData();
});

// 加载下拉数据
const loadDropdownData = async () => {
	try {
		const [typesRes, directionsRes, selectorRes] = await Promise.all([
			finAccountApi.getAccountTypes(),
			finAccountApi.getDirections(),
			finAccountApi.selector()
		]);
		state.accountTypes = typesRes.data.result || [];
		state.directions = directionsRes.data.result || [];
		state.accountSelector = selectorRes.data.result || [];
	} catch (error) {
		console.error('加载下拉数据失败:', error);
	}
};

// 打开弹窗
const openDialog = async (row: any, title: string) => {
	state.title = title;
	row = row ?? {};
	state.ruleForm = row.id ? await finAccountApi.detail(row.id).then(res => res.data.result) : JSON.parse(JSON.stringify(row));
	// 确保默认值设置
	if (!state.ruleForm.id) {
		state.ruleForm = {
			...state.ruleForm,
			isDetail: false,
			isAuxiliary: false,
			isCashFlow: false,
			auxDept: false,
			auxPerson: false,
			auxProject: false,
			auxSupplier: false,
			auxCustomer: false,
			auxInventory: false,
			isEnabled: true,
			level: 1,
		};
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
			await finAccountApi[state.ruleForm.id ? 'update' : 'add'](values);
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
	<div class="finAccount-container">
		<el-dialog v-model="state.showDialog" :width="800" draggable :close-on-click-modal="false">
			<template #header>
				<div style="color: #fff">
					<span>{{ state.title }}</span>
				</div>
			</template>
			<el-form :model="state.ruleForm" ref="ruleFormRef" label-width="120px" :rules="rules">
				<el-row :gutter="35">
					<el-form-item v-show="false">
						<el-input v-model="state.ruleForm.id" />
					</el-form-item>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="科目编码" prop="accountCode">
							<el-input v-model="state.ruleForm.accountCode" placeholder="请输入科目编码" maxlength="50" show-word-limit clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="科目名称" prop="accountName">
							<el-input v-model="state.ruleForm.accountName" placeholder="请输入科目名称" maxlength="100" show-word-limit clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="科目全称" prop="fullName">
							<el-input v-model="state.ruleForm.fullName" placeholder="请输入科目全称" maxlength="200" show-word-limit clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="上级科目" prop="parentId">
							<el-select v-model="state.ruleForm.parentId" clearable filterable placeholder="请选择上级科目" style="width: 100%">
								<el-option v-for="item in state.accountSelector.filter((x: any) => !state.ruleForm.id || x.id !== state.ruleForm.id)" :key="item.id" :label="`${item.accountCode} - ${item.accountName}`" :value="item.id" />
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="科目级次" prop="level">
							<el-input-number v-model="state.ruleForm.level" :min="1" :max="5" placeholder="请选择科目级次" style="width: 100%" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="科目类型" prop="accountType">
							<el-select v-model="state.ruleForm.accountType" clearable filterable placeholder="请选择科目类型" style="width: 100%">
								<el-option v-for="item in state.accountTypes" :key="item.code" :label="item.name" :value="item.code" />
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="余额方向" prop="direction">
							<el-select v-model="state.ruleForm.direction" clearable filterable placeholder="请选择余额方向" style="width: 100%">
								<el-option v-for="item in state.directions" :key="item.code" :label="item.name" :value="item.code" />
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="是否明细科目" prop="isDetail">
							<el-switch v-model="state.ruleForm.isDetail" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="是否辅助核算" prop="isAuxiliary">
							<el-switch v-model="state.ruleForm.isAuxiliary" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="是否现金流量科目" prop="isCashFlow">
							<el-switch v-model="state.ruleForm.isCashFlow" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="部门辅助核算" prop="auxDept">
							<el-switch v-model="state.ruleForm.auxDept" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="个人辅助核算" prop="auxPerson">
							<el-switch v-model="state.ruleForm.auxPerson" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="项目辅助核算" prop="auxProject">
							<el-switch v-model="state.ruleForm.auxProject" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="供应商辅助核算" prop="auxSupplier">
							<el-switch v-model="state.ruleForm.auxSupplier" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="客户辅助核算" prop="auxCustomer">
							<el-switch v-model="state.ruleForm.auxCustomer" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="存货辅助核算" prop="auxInventory">
							<el-switch v-model="state.ruleForm.auxInventory" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="是否启用" prop="isEnabled">
							<el-switch v-model="state.ruleForm.isEnabled" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="排序号" prop="sortOrder">
							<el-input-number v-model="state.ruleForm.sortOrder" :min="0" placeholder="请输入排序号" style="width: 100%" />
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
