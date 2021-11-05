using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TLDAG.Build.Valuating
{
    public class CSharpValuator : SyntaxValuator
    {
        public static CSharpValuator Instance { get; } = new();

        protected CSharpValuator() { }

        public override int Valuate(string source) => Tree(CSharpSyntaxTree.ParseText(source));

        public override int Node(SyntaxNode? node)
            => node is CSharpSyntaxNode csNode ? CSharpNode(csNode) : Unknown(node);

        public virtual int CSharpNode(CSharpSyntaxNode node)
        {
            return node.Kind() switch
            {
                SyntaxKind.AccessorList => AccessorList(node),
                SyntaxKind.AddAccessorDeclaration => AccessorDeclaration(node),
                SyntaxKind.AddAssignmentExpression => AssignmentExpression(node),
                SyntaxKind.AddExpression => BinaryExpression(node),
                SyntaxKind.AddressOfExpression => PrefixUnaryExpression(node),
                SyntaxKind.AndAssignmentExpression => AssignmentExpression(node),
                SyntaxKind.ArgListExpression => LiteralExpression(node),
                SyntaxKind.Argument => Argument(node),
                SyntaxKind.ArgumentList => ArgumentList(node),
                SyntaxKind.ArrayCreationExpression => ArrayCreationExpression(node),
                SyntaxKind.ArrayInitializerExpression => InitializerExpression(node),
                SyntaxKind.ArrayRankSpecifier => ArrayRankSpecifier(node),
                SyntaxKind.ArrayType => ArrayType(node),
                SyntaxKind.ArrowExpressionClause => ArrowExpressionClause(node),
                SyntaxKind.AsExpression => BinaryExpression(node),
                SyntaxKind.Attribute => Attribute(node),
                SyntaxKind.AttributeArgument => AttributeArgument(node),
                SyntaxKind.AttributeArgumentList => AttributeArgumentList(node),
                SyntaxKind.AttributeList => AttributeList(node),
                SyntaxKind.AwaitExpression => AwaitExpression(node),
                SyntaxKind.BaseConstructorInitializer => ConstructorInitializer(node),
                SyntaxKind.BaseList => BaseList(node),
                SyntaxKind.BitwiseAndExpression => BinaryExpression(node),
                SyntaxKind.BitwiseNotExpression => PrefixUnaryExpression(node),
                SyntaxKind.BitwiseOrExpression => BinaryExpression(node),
                SyntaxKind.Block => Block(node),
                SyntaxKind.BracketedArgumentList => BracketedArgumentList(node),
                SyntaxKind.BracketedParameterList => BracketedParameterList(node),
                SyntaxKind.CastExpression => CastExpression(node),
                SyntaxKind.ClassDeclaration => ClassDeclaration(node),
                SyntaxKind.CoalesceAssignmentExpression => AssignmentExpression(node),
                SyntaxKind.CoalesceExpression => BinaryExpression(node),
                SyntaxKind.ConditionalExpression => ConditionalExpression(node),
                SyntaxKind.CharacterLiteralExpression => LiteralExpression(node),
                SyntaxKind.CollectionInitializerExpression => InitializerExpression(node),
                SyntaxKind.CompilationUnit => CompilationUnit(node),
                SyntaxKind.ComplexElementInitializerExpression => InitializerExpression(node),
                SyntaxKind.ConditionalAccessExpression => ConditionalAccessExpression(node),
                SyntaxKind.ConstantPattern => ConstantPattern(node),
                SyntaxKind.ConstructorDeclaration => ConstructorDeclaration(node),
                SyntaxKind.ContinueStatement => ContinueStatement(node),
                SyntaxKind.DeclarationPattern => DeclarationPattern(node),
                SyntaxKind.DefaultLiteralExpression => LiteralExpression(node),
                SyntaxKind.DelegateDeclaration => DelegateDeclaration(node),
                SyntaxKind.DestructorDeclaration => DestructorDeclaration(node),
                SyntaxKind.DivideAssignmentExpression => AssignmentExpression(node),
                SyntaxKind.DivideExpression => BinaryExpression(node),
                SyntaxKind.DoStatement => DoStatement(node),
                SyntaxKind.ElementAccessExpression => ElementAccessExpression(node),
                SyntaxKind.ElseClause => ElseClause(node),
                SyntaxKind.EqualsExpression => BinaryExpression(node),
                SyntaxKind.EqualsValueClause => EqualsValueClause(node),
                SyntaxKind.EventFieldDeclaration => EventFieldDeclaration(node),
                SyntaxKind.ExclusiveOrAssignmentExpression => AssignmentExpression(node),
                SyntaxKind.ExclusiveOrExpression => BinaryExpression(node),
                SyntaxKind.ExplicitInterfaceSpecifier => ExplicitInterfaceSpecifier(node),
                SyntaxKind.ExpressionStatement => ExpressionStatement(node),
                SyntaxKind.FalseLiteralExpression => LiteralExpression(node),
                SyntaxKind.FieldDeclaration => FieldDeclaration(node),
                SyntaxKind.ForEachStatement => ForEachStatement(node),
                SyntaxKind.ForStatement => ForStatement(node),
                SyntaxKind.GenericName => GenericName(node),
                SyntaxKind.GetAccessorDeclaration => AccessorDeclaration(node),
                SyntaxKind.GreaterThanExpression => BinaryExpression(node),
                SyntaxKind.GreaterThanOrEqualExpression => BinaryExpression(node),
                SyntaxKind.IdentifierName => IdentifierName(node),
                SyntaxKind.IfStatement => IfStatement(node),
                SyntaxKind.ImplicitObjectCreationExpression => ImplicitObjectCreationExpression(node),
                SyntaxKind.IndexExpression => PrefixUnaryExpression(node),
                SyntaxKind.IndexerDeclaration => IndexerDeclaration(node),
                SyntaxKind.InitAccessorDeclaration => AccessorDeclaration(node),
                SyntaxKind.InterfaceDeclaration => InterfaceDeclaration(node),
                SyntaxKind.InterpolatedStringExpression => InterpolatedStringExpression(node),
                SyntaxKind.InterpolatedStringText => InterpolatedStringText(node),
                SyntaxKind.Interpolation => Interpolation(node),
                SyntaxKind.InvocationExpression => InvocationExpression(node),
                SyntaxKind.IsExpression => BinaryExpression(node),
                SyntaxKind.IsPatternExpression => IsPatternExpression(node),
                SyntaxKind.LeftShiftAssignmentExpression => AssignmentExpression(node),
                SyntaxKind.LeftShiftExpression => BinaryExpression(node),
                SyntaxKind.LessThanExpression => BinaryExpression(node),
                SyntaxKind.LessThanOrEqualExpression => BinaryExpression(node),
                SyntaxKind.LocalDeclarationStatement => LocalDeclarationStatement(node),
                SyntaxKind.LogicalAndExpression => BinaryExpression(node),
                SyntaxKind.LogicalNotExpression => PrefixUnaryExpression(node),
                SyntaxKind.LogicalOrExpression => BinaryExpression(node),
                SyntaxKind.MemberBindingExpression => MemberBindingExpression(node),
                SyntaxKind.MethodDeclaration => MethodDeclaration(node),
                SyntaxKind.ModuloAssignmentExpression => AssignmentExpression(node),
                SyntaxKind.ModuloExpression => BinaryExpression(node),
                SyntaxKind.MultiplyAssignmentExpression => AssignmentExpression(node),
                SyntaxKind.MultiplyExpression => BinaryExpression(node),
                SyntaxKind.NamespaceDeclaration => NamespaceDeclaration(node),
                SyntaxKind.NotEqualsExpression => BinaryExpression(node),
                SyntaxKind.NotPattern => UnaryPattern(node),
                SyntaxKind.NullLiteralExpression => LiteralExpression(node),
                SyntaxKind.NullableType => NullableType(node),
                SyntaxKind.NumericLiteralExpression => LiteralExpression(node),
                SyntaxKind.ObjectCreationExpression => ObjectCreationExpression(node),
                SyntaxKind.ObjectInitializerExpression => InitializerExpression(node),
                SyntaxKind.OmittedArraySizeExpression => OmittedArraySizeExpression(node),
                SyntaxKind.OperatorDeclaration => OperatorDeclaration(node),
                SyntaxKind.OrAssignmentExpression => AssignmentExpression(node),
                SyntaxKind.Parameter => Parameter(node),
                SyntaxKind.ParameterList => ParameterList(node),
                SyntaxKind.ParenthesizedExpression => ParenthesizedExpression(node),
                SyntaxKind.ParenthesizedLambdaExpression => ParenthesizedLambdaExpression(node),
                SyntaxKind.PointerIndirectionExpression => PrefixUnaryExpression(node),
                SyntaxKind.PointerMemberAccessExpression => MemberAccessExpression(node),
                SyntaxKind.PostDecrementExpression => PostfixUnaryExpression(node),
                SyntaxKind.PostIncrementExpression => PostfixUnaryExpression(node),
                SyntaxKind.PreDecrementExpression => PrefixUnaryExpression(node),
                SyntaxKind.PreIncrementExpression => PrefixUnaryExpression(node),
                SyntaxKind.PredefinedType => PredefinedType(node),
                SyntaxKind.PropertyDeclaration => PropertyDeclaration(node),
                SyntaxKind.QualifiedName => QualifiedName(node),
                SyntaxKind.RemoveAccessorDeclaration => AccessorDeclaration(node),
                SyntaxKind.ReturnStatement => ReturnStatement(node),
                SyntaxKind.RightShiftAssignmentExpression => AssignmentExpression(node),
                SyntaxKind.RightShiftExpression => BinaryExpression(node),
                SyntaxKind.SetAccessorDeclaration => AccessorDeclaration(node),
                SyntaxKind.SimpleAssignmentExpression => AssignmentExpression(node),
                SyntaxKind.SimpleBaseType => SimpleBaseType(node),
                SyntaxKind.SimpleLambdaExpression => SimpleLambdaExpression(node),
                SyntaxKind.SimpleMemberAccessExpression => MemberAccessExpression(node),
                SyntaxKind.SingleVariableDesignation => SingleVariableDesignation(node),
                SyntaxKind.SizeOfExpression => SizeOfExpression(node),
                SyntaxKind.StringLiteralExpression => LiteralExpression(node),
                SyntaxKind.SubtractAssignmentExpression => AssignmentExpression(node),
                SyntaxKind.SubtractExpression => BinaryExpression(node),
                SyntaxKind.SuppressNullableWarningExpression => PostfixUnaryExpression(node),
                SyntaxKind.ThisConstructorInitializer => ConstructorInitializer(node),
                SyntaxKind.ThisExpression => ThisExpression(node),
                SyntaxKind.ThrowExpression => ThrowExpression(node),
                SyntaxKind.ThrowStatement => ThrowStatement(node),
                SyntaxKind.TrueLiteralExpression => LiteralExpression(node),
                SyntaxKind.TypeArgumentList => TypeArgumentList(node),
                SyntaxKind.TypeConstraint => TypeConstraint(node),
                SyntaxKind.TypeParameterConstraintClause => TypeParameterConstraintClause(node),
                SyntaxKind.TypeParameter => TypeParameter(node),
                SyntaxKind.TypeParameterList => TypeParameterList(node),
                SyntaxKind.UnaryMinusExpression => PrefixUnaryExpression(node),
                SyntaxKind.UnaryPlusExpression => PrefixUnaryExpression(node),
                SyntaxKind.UnknownAccessorDeclaration => AccessorDeclaration(node),
                SyntaxKind.UsingDirective => UsingDirective(node),
                SyntaxKind.VariableDeclaration => VariableDeclaration(node),
                SyntaxKind.VariableDeclarator => VariableDeclarator(node),
                SyntaxKind.WithInitializerExpression => InitializerExpression(node),
                SyntaxKind.WhileStatement => WhileStatement(node),
                SyntaxKind.YieldBreakStatement => YieldStatement(node),
                SyntaxKind.YieldReturnStatement => YieldStatement(node),
                _ => Unknown(node)
            };
        }

        public virtual int AAA(CSharpSyntaxNode node)
            => Valuate<AttributeArgumentSyntax>(node, n => 0);

        public virtual int AccessorDeclaration(CSharpSyntaxNode node)
            => Valuate<AccessorDeclarationSyntax>(node, n => Node(n.Body) + Nodes(n.AttributeLists) + Node(n.ExpressionBody));

        public virtual int AccessorList(CSharpSyntaxNode node)
            => Valuate<AccessorListSyntax>(node, n => Nodes(n.Accessors));

        public virtual int AnonymousFunctionExpression(CSharpSyntaxNode node)
            => Valuate<AnonymousFunctionExpressionSyntax>(node, n => Expression(n) + Node(n.Body) + Node(n.Block) + Node(n.ExpressionBody));

        public virtual int Argument(CSharpSyntaxNode node)
            => Valuate<ArgumentSyntax>(node, n => Node(n.NameColon) + Node(n.Expression));

        public virtual int ArgumentList(CSharpSyntaxNode node)
            => Valuate<ArgumentListSyntax>(node, n => BaseArgumentList(n));

        public virtual int ArrayCreationExpression(CSharpSyntaxNode node)
            => Valuate<ArrayCreationExpressionSyntax>(node, n => 1 + Expression(n) + Node(n.Type) + Node(n.Initializer));

        public virtual int ArrayRankSpecifier(CSharpSyntaxNode node)
            => Valuate<ArrayRankSpecifierSyntax>(node, n => Nodes(n.Sizes));

        public virtual int ArrayType(CSharpSyntaxNode node)
            => Valuate<ArrayTypeSyntax>(node, n => Type(n) + Node(n.ElementType) + Nodes(n.RankSpecifiers));

        public virtual int ArrowExpressionClause(CSharpSyntaxNode node)
            => Valuate<ArrowExpressionClauseSyntax>(node, n => 1 + Node(n.Expression));

        public virtual int AssignmentExpression(CSharpSyntaxNode node)
            => Valuate<AssignmentExpressionSyntax>(node, n => 1 + Expression(n) + Node(n.Left) + Node(n.Right));

        public virtual int Attribute(CSharpSyntaxNode node)
            => Valuate<AttributeSyntax>(node, n => 1 + Node(n.Name) + Node(n.ArgumentList));

        public virtual int AttributeArgument(CSharpSyntaxNode node)
            => Valuate<AttributeArgumentSyntax>(node, n => Node(n.NameEquals) + Node(n.NameColon) + Node(n.Expression));

        public virtual int AttributeArgumentList(CSharpSyntaxNode node)
            => Valuate<AttributeArgumentListSyntax>(node, n => Nodes(n.Arguments));

        public virtual int AttributeList(CSharpSyntaxNode node)
            => Valuate<AttributeListSyntax>(node, n => Node(n.Target) + Nodes(n.Attributes));

        public virtual int AwaitExpression(CSharpSyntaxNode node)
            => Valuate<AwaitExpressionSyntax>(node, n => 1 + Expression(n) + Node(n.Expression));

        public virtual int BaseArgumentList(CSharpSyntaxNode node)
            => Valuate<BaseArgumentListSyntax>(node, n => Nodes(n.Arguments));

        public virtual int BaseFieldDeclaration(CSharpSyntaxNode node)
            => Valuate<BaseFieldDeclarationSyntax>(node, n => MemberDeclaration(n) + Node(n.Declaration));

        public virtual int BaseList(CSharpSyntaxNode node)
            => Valuate<BaseListSyntax>(node, n => Nodes(n.Types));

        public virtual int BaseMethodDeclaration(CSharpSyntaxNode node)
            => Valuate<BaseMethodDeclarationSyntax>
            (node, n => MemberDeclaration(n) + Node(n.Body) + Node(n.ExpressionBody) + Node(n.ParameterList));

        public virtual int BaseObjectCreationExpression(CSharpSyntaxNode node)
            => Valuate<BaseObjectCreationExpressionSyntax>(node, n => 1 + Expression(n) + Node(n.ArgumentList) + Node(n.Initializer));

        public virtual int BaseParameter(CSharpSyntaxNode node)
            => Valuate<BaseParameterSyntax>(node, n => Nodes(n.AttributeLists) + Node(n.Type));

        public virtual int BaseParameterList(CSharpSyntaxNode node)
            => Valuate<BaseParameterListSyntax>(node, n => Nodes(n.Parameters));

        public virtual int BasePropertyDeclaration(CSharpSyntaxNode node)
            => Valuate<BasePropertyDeclarationSyntax>
            (node, n => MemberDeclaration(n) + Node(n.Type) + Node(n.ExplicitInterfaceSpecifier) + Node(n.AccessorList));

        public virtual int BaseType(CSharpSyntaxNode node)
            => Valuate<BaseTypeSyntax>(node, n => Node(n.Type));

        public virtual int BaseTypeDeclaration(CSharpSyntaxNode node)
            => Valuate<BaseTypeDeclarationSyntax>(node, n => MemberDeclaration(n) + Node(n.BaseList));

        public virtual int BinaryExpression(CSharpSyntaxNode node)
            => Valuate<BinaryExpressionSyntax>(node, n => Expression(n) + Node(n.Left) + Node(n.Right));

        public virtual int Block(CSharpSyntaxNode node)
            => Valuate<BlockSyntax>(node, n => Statement(n) + Nodes(n.Statements));

        public virtual int BracketedArgumentList(CSharpSyntaxNode node)
            => Valuate<BracketedArgumentListSyntax>(node, n => BaseArgumentList(n));

        public virtual int BracketedParameterList(CSharpSyntaxNode node)
            => Valuate<BracketedParameterListSyntax>(node, n => BaseParameterList(n));

        public virtual int CastExpression(CSharpSyntaxNode node)
            => Valuate<CastExpressionSyntax>(node, n => Expression(n) + Node(n.Type) + Node(n.Expression));

        public virtual int ClassDeclaration(CSharpSyntaxNode node)
            => Valuate<ClassDeclarationSyntax>(node, n => TypeDeclaration(n));

        public virtual int CommonForEachStatement(CSharpSyntaxNode node)
            => Valuate<CommonForEachStatementSyntax>(node, n => Statement(n) + Node(n.Expression) + Node(n.Statement));

        public virtual int CompilationUnit(CSharpSyntaxNode node)
            => Valuate<CompilationUnitSyntax>(node, n => Nodes(n.AttributeLists) + Nodes(n.Usings) + Nodes(n.Externs) + Nodes(n.Members));

        public virtual int ConditionalAccessExpression(CSharpSyntaxNode node)
            => Valuate<ConditionalAccessExpressionSyntax>(node, n => Expression(n) + Node(n.Expression) + Node(n.WhenNotNull));

        public virtual int ConditionalExpression(CSharpSyntaxNode node)
            => Valuate<ConditionalExpressionSyntax>(node, n => Expression(n) + Node(n.Condition) + Node(n.WhenTrue) + Node(n.WhenFalse));

        public virtual int ConstantPattern(CSharpSyntaxNode node)
            => Valuate<ConstantPatternSyntax>(node, n => Pattern(n) + Node(n.Expression));

        public virtual int ConstructorDeclaration(CSharpSyntaxNode node)
            => Valuate<ConstructorDeclarationSyntax>(node, n => BaseMethodDeclaration(n) + Node(n.Initializer));

        public virtual int ConstructorInitializer(CSharpSyntaxNode node)
            => Valuate<ConstructorInitializerSyntax>(node, n => Node(n.ArgumentList));

        public virtual int ContinueStatement(CSharpSyntaxNode node)
            => Valuate<ContinueStatementSyntax>(node, n => Statement(n));

        public virtual int DeclarationPattern(CSharpSyntaxNode node)
            => Valuate<DeclarationPatternSyntax>(node, n => Pattern(n) + Node(n.Type) + Node(n.Designation));

        public virtual int DelegateDeclaration(CSharpSyntaxNode node)
            => Valuate<DelegateDeclarationSyntax>(node, n => MemberDeclaration(n) + Node(n.ParameterList) + Node(n.TypeParameterList)
            + Node(n.ReturnType) + Nodes(n.ConstraintClauses));

        public virtual int DestructorDeclaration(CSharpSyntaxNode node)
            => Valuate<DestructorDeclarationSyntax>(node, n => BaseMethodDeclaration(n));

        public virtual int DoStatement(CSharpSyntaxNode node)
            => Valuate<DoStatementSyntax>(node, n => Statement(n) + Node(n.Condition) + Node(n.Statement));

        public virtual int ElementAccessExpression(CSharpSyntaxNode node)
            => Valuate<ElementAccessExpressionSyntax>(node, n => Expression(n) + Node(n.Expression) + Node(n.ArgumentList));

        public virtual int ElseClause(CSharpSyntaxNode node)
            => Valuate<ElseClauseSyntax>(node, n => Node(n.Statement));

        public virtual int EqualsValueClause(CSharpSyntaxNode node)
            => Valuate<EqualsValueClauseSyntax>(node, n => Node(n.Value));

        public virtual int EventFieldDeclaration(CSharpSyntaxNode node)
            => Valuate<EventFieldDeclarationSyntax>(node, n => BaseFieldDeclaration(n));

        public virtual int ExplicitInterfaceSpecifier(CSharpSyntaxNode node)
            => Valuate<ExplicitInterfaceSpecifierSyntax>(node, n => Node(n.Name));

        public virtual int Expression(CSharpSyntaxNode node)
            => Valuate<ExpressionSyntax>(node, n => ExpressionOrPattern(n));

        public virtual int ExpressionOrPattern(CSharpSyntaxNode node)
            => Valuate<ExpressionOrPatternSyntax>(node, n => 0);

        public virtual int ExpressionStatement(CSharpSyntaxNode node)
            => Valuate<ExpressionStatementSyntax>(node, n => Statement(n) + Node(n.Expression));

        public virtual int FieldDeclaration(CSharpSyntaxNode node)
            => Valuate<FieldDeclarationSyntax>(node, n => BaseFieldDeclaration(n));

        public virtual int ForEachStatement(CSharpSyntaxNode node)
            => Valuate<ForEachStatementSyntax>(node, n => CommonForEachStatement(n) + Node(n.Type));

        public virtual int ForStatement(CSharpSyntaxNode node)
            => Valuate<ForStatementSyntax>(node, n => Statement(n) + Node(n.Statement) + Nodes(n.Incrementors) + Node(n.Condition)
            + Nodes(n.Initializers) + Node(n.Declaration));

        public virtual int GenericName(CSharpSyntaxNode node)
            => Valuate<GenericNameSyntax>(node, n => SimpleName(n) + Node(n.TypeArgumentList));

        public virtual int IdentifierName(CSharpSyntaxNode node)
            => Valuate<IdentifierNameSyntax>(node, n => SimpleName(n));

        public virtual int IfStatement(CSharpSyntaxNode node)
            => Valuate<IfStatementSyntax>(node, n => Statement(n) + Node(n.Else) + Node(n.Condition) + Node(n.Statement));

        public virtual int ImplicitObjectCreationExpression(CSharpSyntaxNode node)
            => Valuate<ImplicitObjectCreationExpressionSyntax>(node, n => BaseObjectCreationExpression(n));

        public virtual int IndexerDeclaration(CSharpSyntaxNode node)
            => Valuate<IndexerDeclarationSyntax>(node, n => BasePropertyDeclaration(n) + Node(n.ParameterList) + Node(n.ExpressionBody));

        public virtual int InitializerExpression(CSharpSyntaxNode node)
            => Valuate<InitializerExpressionSyntax>(node, n => Expression(n) + Nodes(n.Expressions));

        public virtual int InstanceExpression(CSharpSyntaxNode node)
            => Valuate<InstanceExpressionSyntax>(node, n => Expression(n));

        public virtual int InterfaceDeclaration(CSharpSyntaxNode node)
            => Valuate<InterfaceDeclarationSyntax>(node, n => TypeDeclaration(n));

        public virtual int InterpolatedStringContent(CSharpSyntaxNode node)
            => Valuate<InterpolatedStringContentSyntax>(node, n => 0);

        public virtual int InterpolatedStringExpression(CSharpSyntaxNode node)
            => Valuate<InterpolatedStringExpressionSyntax>(node, n => Expression(n) + Nodes(n.Contents));

        public virtual int InterpolatedStringText(CSharpSyntaxNode node)
            => Valuate<InterpolatedStringTextSyntax>(node, n => InterpolatedStringContent(n));

        public virtual int Interpolation(CSharpSyntaxNode node)
            => Valuate<InterpolationSyntax>(node, n => InterpolatedStringContent(n) + Node(n.Expression) + Node(n.AlignmentClause)
            + Node(n.FormatClause));

        public virtual int InvocationExpression(CSharpSyntaxNode node)
            => Valuate<InvocationExpressionSyntax>(node, n => 1 + Expression(n) + Node(n.Expression) + Node(n.ArgumentList));

        public virtual int IsPatternExpression(CSharpSyntaxNode node)
            => Valuate<IsPatternExpressionSyntax>(node, n => Expression(n) + Node(n.Expression) + Node(n.Pattern));

        public virtual int LambdaExpression(CSharpSyntaxNode node)
            => Valuate<LambdaExpressionSyntax>(node, n => AnonymousFunctionExpression(n) + Nodes(n.AttributeLists));

        public virtual int LiteralExpression(CSharpSyntaxNode node)
            => Valuate<LiteralExpressionSyntax>(node, n => Expression(n));

        public virtual int LocalDeclarationStatement(CSharpSyntaxNode node)
            => Valuate<LocalDeclarationStatementSyntax>(node, n => Statement(n) + Node(n.Declaration));

        public virtual int MemberAccessExpression(CSharpSyntaxNode node)
            => Valuate<MemberAccessExpressionSyntax>(node, n => Expression(n) + Node(n.Expression) + Node(n.Name));

        public virtual int MemberBindingExpression(CSharpSyntaxNode node)
            => Valuate<MemberBindingExpressionSyntax>(node, n => Expression(n) + Node(n.Name));

        public virtual int MemberDeclaration(CSharpSyntaxNode node)
            => Valuate<MemberDeclarationSyntax>(node, n => 1 + Nodes(n.AttributeLists));

        public virtual int MethodDeclaration(CSharpSyntaxNode node)
            => Valuate<MethodDeclarationSyntax>
            (node, n => BaseMethodDeclaration(n) + Nodes(n.ConstraintClauses) + Node(n.TypeParameterList)
            + Node(n.ExplicitInterfaceSpecifier) + Node(n.ReturnType));

        public virtual int Name(CSharpSyntaxNode node)
            => Valuate<NameSyntax>(node, n => Type(n));

        public virtual int NamespaceDeclaration(CSharpSyntaxNode node)
            => Valuate<NamespaceDeclarationSyntax>(node, n => 1 + Nodes(n.Members) + Nodes(n.Usings) + Nodes(n.Externs) + Nodes(n.AttributeLists));

        public virtual int NullableType(CSharpSyntaxNode node)
            => Valuate<NullableTypeSyntax>(node, n => Type(n) + Node(n.ElementType));

        public virtual int ObjectCreationExpression(CSharpSyntaxNode node)
            => Valuate<ObjectCreationExpressionSyntax>(node, n => BaseObjectCreationExpression(n) + Node(n.Type));

        public virtual int OmittedArraySizeExpression(CSharpSyntaxNode node)
            => Valuate<OmittedArraySizeExpressionSyntax>(node, n => Expression(n));

        public virtual int OperatorDeclaration(CSharpSyntaxNode node)
            => Valuate<OperatorDeclarationSyntax>(node, n => BaseMethodDeclaration(n) + Node(n.ExplicitInterfaceSpecifier) + Node(n.ReturnType));

        public virtual int Parameter(CSharpSyntaxNode node)
            => Valuate<ParameterSyntax>(node, n => BaseParameter(n) + Node(n.Default));

        public virtual int ParameterList(CSharpSyntaxNode node)
            => Valuate<ParameterListSyntax>(node, n => BaseParameterList(n));

        public virtual int ParenthesizedExpression(CSharpSyntaxNode node)
            => Valuate<ParenthesizedExpressionSyntax>(node, n => Expression(n) + Node(n.Expression));

        public virtual int ParenthesizedLambdaExpression(CSharpSyntaxNode node)
            => Valuate<ParenthesizedLambdaExpressionSyntax>(node, n => LambdaExpression(n) + Node(n.ReturnType) + Node(n.ParameterList));

        public virtual int Pattern(CSharpSyntaxNode node)
            => Valuate<PatternSyntax>(node, n => ExpressionOrPattern(n));

        public virtual int PostfixUnaryExpression(CSharpSyntaxNode node)
            => Valuate<PostfixUnaryExpressionSyntax>(node, n => Expression(n) + Node(n.Operand));

        public virtual int PredefinedType(CSharpSyntaxNode node)
            => Valuate<PredefinedTypeSyntax>(node, n => Type(n));

        public virtual int PrefixUnaryExpression(CSharpSyntaxNode node)
            => Valuate<PrefixUnaryExpressionSyntax>(node, n => Expression(n) + Node(n.Operand));

        public virtual int PropertyDeclaration(CSharpSyntaxNode node)
            => Valuate<PropertyDeclarationSyntax>(node, n => BasePropertyDeclaration(n) + Node(n.Initializer) + Node(n.ExpressionBody));

        public virtual int QualifiedName(CSharpSyntaxNode node)
            => Valuate<QualifiedNameSyntax>(node, n => Name(n) + Node(n.Left) + Node(n.Right));

        public virtual int ReturnStatement(CSharpSyntaxNode node)
            => Valuate<ReturnStatementSyntax>(node, n => Statement(n) + Node(n.Expression));

        public virtual int SimpleBaseType(CSharpSyntaxNode node)
            => Valuate<SimpleBaseTypeSyntax>(node, n => BaseType(n));

        public virtual int SimpleLambdaExpression(CSharpSyntaxNode node)
            => Valuate<SimpleLambdaExpressionSyntax>(node, n => LambdaExpression(n) + Node(n.Parameter));

        public virtual int SimpleName(CSharpSyntaxNode node)
            => Valuate<SimpleNameSyntax>(node, n => Name(n));

        public virtual int SingleVariableDesignation(CSharpSyntaxNode node)
            => Valuate<SingleVariableDesignationSyntax>(node, n => VariableDesignation(n));

        public virtual int SizeOfExpression(CSharpSyntaxNode node)
            => Valuate<SizeOfExpressionSyntax>(node, n => Expression(n) + Node(n.Type));

        public virtual int Statement(CSharpSyntaxNode node)
            => Valuate<StatementSyntax>(node, n => 1 + Nodes(n.AttributeLists));

        public virtual int ThisExpression(CSharpSyntaxNode node)
            => Valuate<ThisExpressionSyntax>(node, n => InstanceExpression(n));

        public virtual int ThrowExpression(CSharpSyntaxNode node)
            => Valuate<ThrowExpressionSyntax>(node, n => 1 + Expression(n) + Node(n.Expression));

        public virtual int ThrowStatement(CSharpSyntaxNode node)
            => Valuate<ThrowStatementSyntax>(node, n => Statement(n) + Node(n.Expression));

        public virtual int Type(CSharpSyntaxNode node)
            => Valuate<TypeSyntax>(node, n => Expression(n));

        public virtual int TypeArgumentList(CSharpSyntaxNode node)
            => Valuate<TypeArgumentListSyntax>(node, n => Nodes(n.Arguments));

        public virtual int TypeConstraint(CSharpSyntaxNode node)
            => Valuate<TypeConstraintSyntax>(node, n => TypeParameterConstraint(n) + Node(n.Type));

        public virtual int TypeDeclaration(CSharpSyntaxNode node)
            => Valuate<TypeDeclarationSyntax>(node, n => BaseTypeDeclaration(n) + Nodes(n.Members));

        public virtual int TypeParameter(CSharpSyntaxNode node)
            => Valuate<TypeParameterSyntax>(node, n => Nodes(n.AttributeLists));

        public virtual int TypeParameterConstraint(CSharpSyntaxNode node)
            => Valuate<TypeParameterConstraintSyntax>(node, n => 0);

        public virtual int TypeParameterConstraintClause(CSharpSyntaxNode node)
            => Valuate<TypeParameterConstraintClauseSyntax>(node, n => 1 + Node(n.Name) + Nodes(n.Constraints));

        public virtual int TypeParameterList(CSharpSyntaxNode node)
            => Valuate<TypeParameterListSyntax>(node, n => Nodes(n.Parameters));

        public virtual int UnaryPattern(CSharpSyntaxNode node)
            => Valuate<UnaryPatternSyntax>(node, n => Pattern(n) + Node(n.Pattern));

        public virtual int UsingDirective(CSharpSyntaxNode node)
            => Valuate<UsingDirectiveSyntax>(node, n => 1);

        public virtual int VariableDeclaration(CSharpSyntaxNode node)
            => Valuate<VariableDeclarationSyntax>(node, n => Node(n.Type) + Nodes(n.Variables));

        public virtual int VariableDeclarator(CSharpSyntaxNode node)
            => Valuate<VariableDeclaratorSyntax>(node, n => Node(n.ArgumentList) + Node(n.Initializer));

        public virtual int VariableDesignation(CSharpSyntaxNode node)
            => Valuate<VariableDesignationSyntax>(node, n => 0);

        public virtual int WhileStatement(CSharpSyntaxNode node)
            => Valuate<WhileStatementSyntax>(node, n => Statement(n) + Node(n.Statement) + Node(n.Condition));

        public virtual int YieldStatement(CSharpSyntaxNode node)
            => Valuate<YieldStatementSyntax>(node, n => Statement(n) + Node(n.Expression));

    }
}