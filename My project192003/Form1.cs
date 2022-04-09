
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.DataSourcesGDB;
//using ESRI.ArcGIS.DataSourcesRaster;
//using ESRI.ArcGIS.GeoAnalyst;
using ESRI.ArcGIS.esriSystem;
//using ESRI.ArcGIS.SpatialAnalyst;


namespace My_project192003
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void axMapControl1_OnMouseMove(object sender, IMapControlEvents2_OnMouseMoveEvent e)
        {
            IPoint Ppoint = axMapControl1.ToMapPoint(e.x, e.y);
            statusStrip1.Items[0].Text = "X=" + Ppoint.X.ToString() + ",  Y=" + Ppoint.X.ToString();

        }

        IFeatureLayer pTocFealay = null;

        //右键菜单
        private void axTOCControl1_OnMouseDown(object sender, ITOCControlEvents_OnMouseDownEvent e)
        {
            if (e.button == 2)
            {
                esriTOCControlItem pIeam = esriTOCControlItem.esriTOCControlItemNone;
                IBasicMap pMap = null;
                object unk = null;
                object data = null;
                ILayer pLayer = null;
                axTOCControl1.HitTest(e.x, e.y, ref pIeam, ref pMap, ref pLayer, ref unk, ref data);
                pTocFealay = pLayer as IFeatureLayer;
                if (pIeam == esriTOCControlItem.esriTOCControlItemLayer && pTocFealay != null)
                { contextMenuStrip1.Show(Control.MousePosition);}
            }
        }

        //移除图层功能
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (pTocFealay == null) return;
            (axMapControl1.Map as IActiveView).Extent = pTocFealay.AreaOfInterest;
            (axMapControl1.Map as IActiveView).PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
        }

        //删除功能
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {

            if (pTocFealay == null) return;
            DialogResult result = MessageBox.Show("是否删除" + pTocFealay.Name + "图层", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (result == DialogResult.OK)
            {
                axMapControl1.Map.DeleteLayer(pTocFealay);
            }
        }
        Attribu attribute;
        //属性表
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (attribute == null || attribute.IsDisposed)
            {
                attribute = new Attribu();
            }
            //传入当前图层信息
            try
            {
                if (pTocFealay.FeatureClass.Fields == null) ;
                attribute.CurFeatureLayer = pTocFealay;
                // 初始化显示窗体
                attribute.InitUI();
                attribute.ShowDialog();
            }
            catch {
                MessageBox.Show("当前图层有问题");
            }
            return;
        }




        private void 法1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ICommand pCom = new ControlsOpenDocCommand();
            pCom.OnCreate(axMapControl1.Object);
            pCom.OnClick();

        }

        private void 法2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //加载数据前如果有数据则清空
            OpenFileDialog pOpenFileDialog = new OpenFileDialog();
            pOpenFileDialog.CheckFileExists = true;
            pOpenFileDialog.Title = "打开地图文档";
            pOpenFileDialog.Filter = "ArcMap文档(*.mxd)|*.mxd;|ArcMap模板(*.mxt)|*.mxt|发布地图文件(*.pmf)|*.pmf|所有地图格式(*.mxd;*.mxt;*.pmf)|*.mxd;*.mxt;*.pmf";
            pOpenFileDialog.Multiselect = false; //不允许多个文件同时选择
            pOpenFileDialog.RestoreDirectory = true; //存储打开的文件路径
            if (pOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                string pFileName = pOpenFileDialog.FileName;
                if (pFileName == "") return;
                if (axMapControl1.CheckMxFile(pFileName)) //检查地图文档有效性
                {
                    axMapControl1.LoadMxFile(pFileName);
                }
                else
                {
                    MessageBox.Show(pFileName + "是无效的地图文档!", "信息提示");
                    return;
                }
                axTOCControl1.Refresh();
            }
        }
        //显示坐标
        private void axMapControl1_OnMouseMove_1(object sender, IMapControlEvents2_OnMouseMoveEvent e)
        {
            IPoint Ppoint = axMapControl1.ToMapPoint(e.x, e.y);
            statusStrip1.Items[0].Text = "X=" + Ppoint.X.ToString() + ",  Y=" + Ppoint.X.ToString();
        }
    }
}
