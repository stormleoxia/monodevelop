﻿//
// SolutionItemExtension.cs
//
// Author:
//       Lluis Sanchez Gual <lluis@xamarin.com>
//
// Copyright (c) 2014 Xamarin, Inc (http://www.xamarin.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using System.Collections.Generic;
using MonoDevelop.Core;
using MonoDevelop.Core.StringParsing;
using MonoDevelop.Core.Execution;
using System.Xml;
using MonoDevelop.Projects.Formats.MSBuild;
using MonoDevelop.Projects.Extensions;
using System.Threading.Tasks;
using System.Linq;

namespace MonoDevelop.Projects
{
	public class SolutionItemExtension: WorkspaceObjectExtension
	{
		SolutionItemExtension next;

		internal string FlavorGuid { get; set; }

		internal protected override void InitializeChain (ChainedExtension next)
		{
			base.InitializeChain (next);
			this.next = FindNextImplementation<SolutionItemExtension> (next);
		}

		internal protected override bool SupportsObject (WorkspaceObject item)
		{
			var p = item as SolutionItem;
			if (p == null)
				return false;

			return FlavorGuid == null || p.GetItemTypeGuids ().Any (id => id.Equals (FlavorGuid, StringComparison.OrdinalIgnoreCase));
		}

		public SolutionItem Item {
			get { return (SolutionItem) Owner; }
		}

		internal protected virtual void OnInitializeNew (string languageName, ProjectCreateInformation info, XmlElement projectOptions)
		{
			next.OnInitializeNew (languageName, info, projectOptions);
		}

		internal protected virtual void OnInitializeFromTemplate (XmlElement template)
		{
			next.OnInitializeFromTemplate (template);
		}

		internal void BeginLoad ()
		{
			OnBeginLoad ();
			if (next != null)
				next.BeginLoad ();
		}

		internal void EndLoad ()
		{
			OnEndLoad ();
			if (next != null)
				next.OnEndLoad ();
		}

		#region Project properties

		internal protected virtual IconId StockIcon {
			get {
				return next.StockIcon;
			}
		}
		#endregion

		#region Project model

		internal protected virtual FilePath OnGetDefaultBaseDirectory ()
		{
			return next.OnGetDefaultBaseDirectory ();
		}

		internal protected virtual IEnumerable<IBuildTarget> OnGetExecutionDependencies ()
		{
			return next.OnGetExecutionDependencies ();
		}

		internal protected virtual IEnumerable<SolutionItem> OnGetReferencedItems (ConfigurationSelector configuration)
		{
			return next.OnGetReferencedItems (configuration);
		}

		internal protected virtual StringTagModelDescription OnGetStringTagModelDescription (ConfigurationSelector conf)
		{
			return next.OnGetStringTagModelDescription (conf);
		}

		internal protected virtual StringTagModel OnGetStringTagModel (ConfigurationSelector conf)
		{
			return next.OnGetStringTagModel (conf);
		}

		internal protected virtual bool OnGetSupportsFormat (FileFormat format)
		{
			return next.OnGetSupportsFormat (format);
		}

		internal protected virtual IEnumerable<FilePath> OnGetItemFiles (bool includeReferencedFiles)
		{
			return next.OnGetItemFiles (includeReferencedFiles);
		}

		internal protected virtual bool ItemFilesChanged {
			get {
				return next.ItemFilesChanged;
			}
		}

		internal protected virtual SolutionItemConfiguration OnCreateConfiguration (string name)
		{
			return next.OnCreateConfiguration (name);
		}

		internal protected virtual string[] SupportedPlatforms {
			get {
				return next.SupportedPlatforms;
			}
		}

		internal protected virtual ProjectFeatures OnGetSupportedFeatures ()
		{
			return next.OnGetSupportedFeatures ();
		}

		#endregion

		#region Building

		internal protected virtual Task<BuildResult> OnClean (ProgressMonitor monitor, ConfigurationSelector configuration)
		{
			return next.OnClean (monitor, configuration);
		}

		internal protected virtual bool OnNeedsBuilding (ConfigurationSelector configuration)
		{
			return next.OnNeedsBuilding (configuration);
		}

		internal protected virtual Task<BuildResult> OnBuild (ProgressMonitor monitor, ConfigurationSelector configuration)
		{
			return next.OnBuild (monitor, configuration);
		}

		internal protected virtual DateTime OnGetLastBuildTime (ConfigurationSelector configuration)
		{
			return next.OnGetLastBuildTime (configuration);
		}

		#endregion

		#region Load / Save

		internal protected virtual Task OnLoad (ProgressMonitor monitor)
		{
			return next.OnLoad (monitor);
		}

		internal protected virtual Task OnSave (ProgressMonitor monitor)
		{
			return next.OnSave (monitor);
		}

		protected virtual void OnBeginLoad ()
		{
		}

		protected virtual void OnEndLoad ()
		{
		}

		internal protected virtual void OnReadSolutionData (ProgressMonitor monitor, SlnPropertySet properties)
		{
			next.OnReadSolutionData (monitor, properties);
		}

		internal protected virtual void OnWriteSolutionData (ProgressMonitor monitor, SlnPropertySet properties)
		{
			next.OnWriteSolutionData (monitor, properties);
		}

		internal protected virtual bool OnCheckHasSolutionData ()
		{
			return next.OnCheckHasSolutionData ();
		}

		#endregion

		#region Execution

		internal protected virtual Task OnExecute (ProgressMonitor monitor, ExecutionContext context, ConfigurationSelector configuration)
		{
			return next.OnExecute (monitor, context, configuration);
		}

		internal protected virtual bool OnGetCanExecute (ExecutionContext context, ConfigurationSelector configuration)
		{
			return next.OnGetCanExecute (context, configuration);
		}

		internal protected virtual IEnumerable<ExecutionTarget> OnGetExecutionTargets (ConfigurationSelector configuration)
		{
			return next.OnGetExecutionTargets (configuration);
		}

		internal protected virtual void OnExecutionTargetsChanged ()
		{
			next.OnExecutionTargetsChanged ();
		}

		#endregion

		#region Events

		internal protected virtual void OnReloadRequired (SolutionItemEventArgs args)
		{
			next.OnReloadRequired (args);
		}

		internal protected virtual void OnItemsAdded (IEnumerable<ProjectItem> objs)
		{
			next.OnItemsAdded (objs);
		}

		internal protected virtual void OnItemsRemoved (IEnumerable<ProjectItem> objs)
		{
			next.OnItemsRemoved (objs);
		}

		internal protected virtual void OnDefaultConfigurationChanged (ConfigurationEventArgs args)
		{
			next.OnDefaultConfigurationChanged (args);
		}

		internal protected virtual void OnBoundToSolution ()
		{
			next.OnBoundToSolution ();
		}

		internal protected virtual void OnUnboundFromSolution ()
		{
			next.OnUnboundFromSolution ();
		}

		internal protected virtual void OnConfigurationAdded (ConfigurationEventArgs args)
		{
			next.OnConfigurationAdded (args);
		}

		internal protected virtual void OnConfigurationRemoved (ConfigurationEventArgs args)
		{
			next.OnConfigurationRemoved (args);
		}

		internal protected virtual void OnModified (SolutionItemModifiedEventArgs args)
		{
			next.OnModified (args);
		}

		internal protected virtual void OnNameChanged (SolutionItemRenamedEventArgs e)
		{
			next.OnNameChanged (e);
		}

		#endregion
	}
}
