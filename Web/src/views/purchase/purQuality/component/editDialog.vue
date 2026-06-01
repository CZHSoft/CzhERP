<script lang="ts" setup name="purQuality">
import { ref, reactive, onMounted } from 'vue';
import { ElMessage } from 'element-plus';
import type { FormRules } from 'element-plus';
import { formatDate } from '/@/utils/formatTime';
import { usePurQualityApi } from '/@/api/purchase/purQuality';
import { usePurInboundApi } from '/@/api/purchase/purInbound';
import { getAPI } from '/@/utils/axios-utils';
import { SysUserApi } from '/@/api-services/api';

const emit = defineEmits(['reloadTable']);
const purQualityApi = usePurQualityApi();
const purInboundApi = usePurInboundApi();
const ruleFormRef = ref();

const state = reactive({
  title: '',
  loading: false,
  showDialog: false,
  ruleForm: {} as any,
  inboundOptions: [] as any[],
  inspectionTypeOptions: [
    { label: '全检', value: 1 },
    { label: '抽检', value: 2 }
  ],
  resultOptions: [
    { label: '待判定', value: 0 },
    { label: '合格', value: 1 },
    { label: '不合格', value: 2 },
    { label: '让步接收', value: 3 }
  ],
  stores: {},
  dropdownData: {} as any,
});

const rules = ref<FormRules>({
  qualityNo: [{ required: true, message: '质检单号不能为空', trigger: 'blur' }],
  inboundId: [{ required: true, message: '请选择采购入库单', trigger: 'change' }],
  inspectionType: [{ required: true, message: '请选择检验类型', trigger: 'change' }],
  passQty: [{ required: true, message: '合格数量不能为空', trigger: 'blur' }],
  failQty: [{ required: true, message: '不合格数量不能为空', trigger: 'blur' }],
  result: [{ required: true, message: '请选择检验结果', trigger: 'change' }],
  inspectorId: [{ required: true, message: '检验员不能为空', trigger: 'change' }],
  inspectionDate: [{ required: true, message: '请选择检验日期', trigger: 'change' }],
});

onMounted(async () => {
  await loadInboundOptions();
});

const loadInboundOptions = async () => {
  try {
    const result = await purInboundApi.list().then(res => res.data.result);
    state.inboundOptions = result?.map((item: any) => ({
      label: `${item.inboundNo} - ${item.supplierName || ''}`,
      value: item.id,
      inboundNo: item.inboundNo
    })) ?? [];
  } catch (e) {
    console.error('加载入库单列表失败', e);
    state.inboundOptions = [];
  }
};

const getCurrentUserId = async () => {
  try {
    const result = await getAPI(SysUserApi).apiSysUserUserinfoGet();
    return result.data.result?.userId;
  } catch (e) {
    console.error('获取当前用户失败', e);
    return 0;
  }
};

const openDialog = async (row: any, title: string) => {
  state.title = title;
  state.loading = true;
  state.showDialog = true;

  if (row?.id) {
    state.ruleForm = await purQualityApi.detail(row.id).then(res => res.data.result);
  } else {
    const nextCode = await purQualityApi.getNextCode().then(res => res.data.result);
    const userId = await getCurrentUserId();
    state.ruleForm = {
      id: null,
      qualityNo: nextCode,
      inboundId: null,
      inspectionType: 1,
      sampleQty: 0,
      passQty: 0,
      failQty: 0,
      result: 0,
      inspectorId: userId,
      inspectionDate: formatDate(new Date(), 'YYYY-mm-dd'),
      remark: ''
    };
  }

  state.loading = false;
};

const closeDialog = () => {
  emit('reloadTable');
  state.showDialog = false;
};

const submit = async () => {
  ruleFormRef.value.validate(async (isValid: boolean, fields?: any) => {
    if (isValid) {
      let values = state.ruleForm;
      await purQualityApi[state.ruleForm.id ? 'update' : 'add'](values);
      closeDialog();
    } else {
      ElMessage({
        message: `表单有${Object.keys(fields).length}处验证失败，请修改后再提交`,
        type: 'error',
      });
    }
  });
};

defineExpose({ openDialog });
</script>
<template>
  <div class="purQuality-container">
    <el-dialog v-model="state.showDialog" :width="800" draggable :close-on-click-modal="false">
      <template #header>
        <div style="color: #fff">
          <span>{{ state.title }}</span>
        </div>
      </template>
      <el-form :model="state.ruleForm" ref="ruleFormRef" label-width="100" :rules="rules">
        <el-row :gutter="35">
          <el-form-item v-show="false">
            <el-input v-model="state.ruleForm.id" />
          </el-form-item>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
            <el-form-item label="质检单号" prop="qualityNo">
              <el-input v-model="state.ruleForm.qualityNo" placeholder="系统自动生成" disabled />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
            <el-form-item label="采购入库单" prop="inboundId">
              <el-select v-model="state.ruleForm.inboundId" placeholder="请选择采购入库单" clearable filterable style="width: 100%">
                <el-option v-for="item in state.inboundOptions" :key="item.value" :label="item.label" :value="item.value" />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
            <el-form-item label="检验类型" prop="inspectionType">
              <el-select v-model="state.ruleForm.inspectionType" placeholder="请选择检验类型" style="width: 100%">
                <el-option v-for="item in state.inspectionTypeOptions" :key="item.value" :label="item.label" :value="item.value" />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
            <el-form-item label="抽样数量" prop="sampleQty">
              <el-input-number v-model="state.ruleForm.sampleQty" :precision="2" :min="0" placeholder="抽样数量" style="width: 100%" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
            <el-form-item label="合格数量" prop="passQty">
              <el-input-number v-model="state.ruleForm.passQty" :precision="2" :min="0" placeholder="合格数量" style="width: 100%" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
            <el-form-item label="不合格数量" prop="failQty">
              <el-input-number v-model="state.ruleForm.failQty" :precision="2" :min="0" placeholder="不合格数量" style="width: 100%" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
            <el-form-item label="检验结果" prop="result">
              <el-select v-model="state.ruleForm.result" placeholder="请选择检验结果" style="width: 100%">
                <el-option v-for="item in state.resultOptions" :key="item.value" :label="item.label" :value="item.value" />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
            <el-form-item label="检验员ID" prop="inspectorId">
              <el-input-number v-model="state.ruleForm.inspectorId" :min="0" placeholder="检验员ID" style="width: 100%" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
            <el-form-item label="检验日期" prop="inspectionDate">
              <el-date-picker v-model="state.ruleForm.inspectionDate" type="date" placeholder="请选择检验日期" value-format="YYYY-MM-DD" style="width: 100%" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
            <el-form-item label="备注" prop="remark">
              <el-input v-model="state.ruleForm.remark" placeholder="请输入备注" maxlength="500" show-word-limit clearable type="textarea" :rows="3" />
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
      <template #footer>
        <span class="dialog-footer">
          <el-button @click="state.showDialog = false"> 取消 </el-button>
          <el-button @click="submit" type="primary" v-reclick="1000" :loading="state.loading"> 确定 </el-button>
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
