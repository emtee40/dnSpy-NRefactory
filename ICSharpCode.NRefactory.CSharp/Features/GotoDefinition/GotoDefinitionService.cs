﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.FindSymbols;
using Microsoft.CodeAnalysis.LanguageServices;
using Microsoft.CodeAnalysis.Shared.Extensions;
using Roslyn.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace ICSharpCode.NRefactory6.CSharp.Features.GotoDefinition
{
	public static class GoToDefinitionService
	{
		/// <summary>
		/// Navigate to the first source location of a given symbol.
		/// bool TryNavigateToSymbol(ISymbol symbol, Project project, bool usePreviewTab = false);
		/// </summary>
		public static Func<ISymbol, Project, bool, bool> TryNavigateToSymbol = delegate {
			return true;
		};

		/// <summary>
		/// Navigates to the given position in the specified document, opening it if necessary.
		/// bool TryNavigateToSpan(Workspace workspace, DocumentId documentId, TextSpan textSpan, bool usePreviewTab = false);
		/// </summary>
		public static Func<Workspace, DocumentId, TextSpan, bool, bool> TryNavigateToSpan = delegate {
			return true;
		};

		/// <summary>
		/// Determines whether it is possible to navigate to the given position in the specified document.
		/// bool CanNavigateToSpan(Workspace workspace, DocumentId documentId, TextSpan textSpan);
		/// </summary>
		public static Func<Workspace, DocumentId, TextSpan, bool> CanNavigateToSpan = delegate {
			return true;
		};

		public static Action<IEnumerable<Tuple<Solution, ISymbol, Location>>> DisplayMultiple = delegate {
		};

		/// <summary>
		/// bool TrySymbolNavigationNotify(ISymbol symbol, Solution solution);
		/// </summary>
		/// <returns>True if the navigation was handled, indicating that the caller should not 
		/// perform the navigation.
		/// 
		/// </returns>
		public static Func<ISymbol, Solution, bool> TrySymbolNavigationNotify = delegate {
			return false;
		};

		static ISymbol FindRelatedExplicitlyDeclaredSymbol(ISymbol symbol, Compilation compilation)
		{
			return symbol;
		}

		static async Task<ISymbol> FindSymbolAsync(Document document, int position, CancellationToken cancellationToken)
		{
			var workspace = document.Project.Solution.Workspace;

			var semanticModel = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);
			//var symbol = SymbolFinder.FindSymbolAtPosition(semanticModel, position, workspace, bindLiteralsToUnderlyingType: true, cancellationToken: cancellationToken);
			var symbol = SymbolFinder.FindSymbolAtPosition(semanticModel, position, workspace, cancellationToken: cancellationToken);

			return FindRelatedExplicitlyDeclaredSymbol(symbol, semanticModel.Compilation);
		}

//		public async Task<IEnumerable<INavigableItem>> FindDefinitionsAsync(Document document, int position, CancellationToken cancellationToken)
//		{
//			var symbol = await FindSymbolAsync(document, position, cancellationToken).ConfigureAwait(false);
//
//			// realize the list here so that the consumer await'ing the result doesn't lazily cause
//			// them to be created on an inappropriate thread.
//			return NavigableItemFactory.GetItemsfromPreferredSourceLocations(document.Project.Solution, symbol).ToList();
//		}

		public static bool TryGoToDefinition(Document document, int position, CancellationToken cancellationToken)
		{
			var symbol = FindSymbolAsync(document, position, cancellationToken).Result;

			if (symbol != null)
			{
				var containingTypeSymbol = GetContainingTypeSymbol(position, document, cancellationToken);

				if (GoToDefinitionHelpers.TryGoToDefinition(symbol, document.Project, containingTypeSymbol, throwOnHiddenDefinition: true, cancellationToken: cancellationToken))
				{
					return true;
				}
			}

			return false;
		}

		private static ITypeSymbol GetContainingTypeSymbol(int caretPosition, Document document, CancellationToken cancellationToken)
		{
			var syntaxRoot = document.GetSyntaxRootAsync(cancellationToken).Result;
			var containingTypeDeclaration = syntaxRoot.GetContainingTypeDeclaration(caretPosition);

			if (containingTypeDeclaration != null)
			{
				var semanticModel = document.GetSemanticModelAsync(cancellationToken).Result;
				return semanticModel.GetDeclaredSymbol(containingTypeDeclaration, cancellationToken) as ITypeSymbol;
			}

			return null;
		}
	}
}