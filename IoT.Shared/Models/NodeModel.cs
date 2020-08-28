using Microsoft.Extensions.Primitives;
using IoT.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace IoT.Shared.Models
{
    public class NodeModel
    {
        public NodeModel()
        {
        }

        public string DisplayName
        {
            get { return NodeEntity.DisplayName; }
            set { NodeEntity.DisplayName = value; }
        }

        public RunState RunState
        {
            get { return NodeEntity.RunState; }
            set { NodeEntity.RunState = value; }
        }

        public string ScriptText
        {

            get{return NodeEntity.ScriptText; }
            set{ NodeEntity.ScriptText = value; }
        }

        public string ScriptType
        {

            get{return NodeEntity.ScriptType; }
            set{ NodeEntity.ScriptType = value; }
        }

        public string MessageTemplateText
        {
            get {return NodeEntity.MessageTemplateText; }
            set {NodeEntity.MessageTemplateText = value; }
        }

        public string MessageTemplateType
        {
            get {return NodeEntity.MessageTemplateType; }
            set {NodeEntity.MessageTemplateType = value; }
        }

        public NodeEntity NodeEntity {get; private set; }
           

        public static NodeModel Create(NodeEntity nodeEntity)
        {
            NodeModel model = new NodeModel();

            model.NodeEntity = nodeEntity;

            return model;
        }
    }
}
