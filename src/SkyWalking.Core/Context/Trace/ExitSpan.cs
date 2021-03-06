﻿/*
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The ASF licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 */

using System;
using System.Collections.Generic;
using System.Text;
using SkyWalking.Dictionarys;
using SkyWalking.NetworkProtocol;
using SkyWalking.NetworkProtocol.Trace;

namespace SkyWalking.Context.Trace
{
    public class ExitSpan : StackBasedTracingSpan, IWithPeerInfo
    {
        private readonly string _peer;
        private readonly int _peerId;

        public ExitSpan(int spanId, int parentSpanId, String operationName, String peer)
            : base(spanId, parentSpanId, operationName)
        {
            _peer = peer;
            _peerId = DictionaryUtil.NullValue;
        }

        public ExitSpan(int spanId, int parentSpanId, int operationId, int peerId)
            : base(spanId, parentSpanId, operationId)
        {
            _peer = null;
            _peerId = peerId;
        }

        public ExitSpan(int spanId, int parentSpanId, int operationId, String peer)
            : base(spanId, parentSpanId, operationId)
        {
            _peer = peer;
            _peerId = DictionaryUtil.NullValue;
        }

        public ExitSpan(int spanId, int parentSpanId, String operationName, int peerId)
            : base(spanId, parentSpanId, operationName)
        {
            _peer = null;
            _peerId = peerId;
        }

        public override bool IsEntry => false;

        public override bool IsExit => true;

        public int PeerId => _peerId;

        public string Peer => _peer;

        public override ISpan Start()
        {
            if (++_stackDepth == 1)
            {
                base.Start();
            }

            return base.Start();
        }

        public override ISpan Tag(string key, string value)
        {
            if (_stackDepth == 1)
            {
                base.Tag(key, value);
            }

            return this;
        }

        public override ISpan SetLayer(SpanLayer layer)
        {
            if (_stackDepth == 1)
            {
                return base.SetLayer(layer);
            }

            return this;
        }

        public override ISpan SetComponent(IComponent component)
        {
            if (_stackDepth == 1)
            {
                return base.SetComponent(component);
            }

            return this;
        }

        public override ISpan SetComponent(string componentName)
        {
            if (_stackDepth == 1)
            {
                return base.SetComponent(componentName);
            }

            return this;
        }

        public override string OperationName
        {
            get { return base.OperationName; }
            set
            {
                if (_stackDepth == 1)
                {
                    base.OperationName = value;
                }
            }
        }

        public override int OperationId
        {
            get { return base.OperationId; }
            set
            {
                if (_stackDepth == 1)
                {
                    base.OperationId = value;
                }
            }
        }

        public override SpanObject Transform()
        {
            var spanObject = base.Transform();

            if (_peerId != DictionaryUtil.NullValue)
            {
                spanObject.PeerId = _peerId;
            }
            else
            {
                spanObject.Peer = _peer;
            }

            return spanObject;
        }
    }
}
